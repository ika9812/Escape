using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    CountDown,
    Playing,
    Finish
}
public class Player : MonoBehaviour
{
    [Header("移動")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpPower = 8f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _gravity = -20f;

    [Header("攻撃")]
    [SerializeField] private float _fireSpeed = 10f;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Vector3 _fireOffset;

    [Header("ステータス")]
    public int _maxHp = 100;
    public ReactiveProperty<int> _life { get; private set; } = new();
    [SerializeField] private float _knockbackSpeed = 10f;
    [SerializeField] private float _knockbackDuration = 0.2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _goalText;
    [SerializeField] private TextMeshProUGUI _countDownText;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("状態")]
    [SerializeField] private PlayerState _state;

    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private Vector2 _moveInput;
    private Vector3 _verticalVelocity;
    private Vector3 _knockbackVelocity;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Collider _goalCollider;
    private bool _isGoal;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _startAction;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
        _startAction = _playerInput.actions["Start"];
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        _jumpAction.performed += OnJump;
        _attackAction.performed += OnAttack;
        _startAction.performed += OnStart;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;

        _jumpAction.performed -= OnJump;
        _attackAction.performed -= OnAttack;
        _startAction.performed -= OnStart;
    }

    private async UniTaskVoid Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        _life.Value = _maxHp;

        _goalText.text = string.Empty;
        _countDownText.text = string.Empty;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        await StartCountDownAsync();
    }

    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Playing:
                Move();
                ApplyGravity();
                ApplyKnockback();
                break;

            case PlayerState.Finish:
                Goal();
                break;
        }
    }

    private async UniTask StartCountDownAsync()
    {
        _state = PlayerState.CountDown;
        _moveInput = Vector2.zero;

        await UniTask.Delay(
            1000,
            cancellationToken: destroyCancellationToken
        );

        string[] countTexts = { "3", "2", "1", "Go!" };

        foreach (string text in countTexts)
        {
            _countDownText.text = text;

            await UniTask.Delay(
                1000,
                cancellationToken: destroyCancellationToken
            );
        }

        _countDownText.text = string.Empty;
        _state = PlayerState.Playing;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (_state != PlayerState.Playing)
        {
            _moveInput = Vector2.zero;
            return;
        }

        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_state != PlayerState.Playing)
            return;

        if (_characterController.isGrounded)
        {
            _verticalVelocity.y = _jumpPower;

            if (_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_state != PlayerState.Playing)
            return;

        if (_firePrefab == null)
            return;

        Vector3 spawnPosition =
            transform.position + transform.TransformVector(_fireOffset);

        GameObject fireObject = Instantiate(
            _firePrefab,
            spawnPosition,
            transform.rotation
        );

        Rigidbody fireRigidbody = fireObject.GetComponent<Rigidbody>();

        if (fireRigidbody != null)
        {
            fireRigidbody.linearVelocity =
                transform.forward * _fireSpeed;
        }

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void OnStart(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_state == PlayerState.Finish && _isGoal)
        {
            ResetPlayer();
        }
    }

    private void Move()
    {
        Camera inputCamera = _playerInput.camera;

        if (inputCamera == null)
            return;

        Vector3 cameraForward = inputCamera.transform.forward;
        Vector3 cameraRight = inputCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction =
            cameraRight * _moveInput.x +
            cameraForward * _moveInput.y;

        direction = Vector3.ClampMagnitude(direction, 1f);

        _characterController.Move(direction * _speed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotateSpeed * Time.deltaTime
            );
        }

        if (_animator != null)
        {
            _animator.SetFloat("MoveSpeed",direction.magnitude);
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded &&
            _verticalVelocity.y < 0f)
        {
            _verticalVelocity.y = -2f;
        }

        _verticalVelocity.y += _gravity * Time.deltaTime;

        _characterController.Move(
            _verticalVelocity * Time.deltaTime
        );
    }

    private void ApplyKnockback()
    {
        if (_knockbackVelocity.sqrMagnitude < 0.01f)
        {
            _knockbackVelocity = Vector3.zero;
            return;
        }

        _characterController.Move(
            _knockbackVelocity * Time.deltaTime
        );

        _knockbackVelocity = Vector3.Lerp(
            _knockbackVelocity,
            Vector3.zero,
            Time.deltaTime / Mathf.Max(_knockbackDuration, 0.01f)
        );
    }

    private void OnControllerColliderHit(
        ControllerColliderHit hit
    )
    {
        if (_state != PlayerState.Playing)
            return;

        if (hit.gameObject.CompareTag("Enemy"))
        {
            AttackObject attackObject = hit.gameObject.GetComponent<AttackObject>();

            if (attackObject != null)
            {
                TakeDamage(
                    attackObject.power,
                    hit.transform.position
                );
            }
        }

        if (hit.gameObject.name == "Goal")
        {
            ReachGoal(hit.collider);
        }
    }

    private void TakeDamage(
        int damage,
        Vector3 enemyPosition
    )
    {
        _life.Value -= damage;

        if (_life.Value <= 0)
        {
            Die();
            return;
        }

        Vector3 knockbackDirection =
            transform.position - enemyPosition;

        knockbackDirection.y = 0f;

        if (knockbackDirection.sqrMagnitude < 0.001f)
        {
            knockbackDirection = -transform.forward;
        }

        _knockbackVelocity =
            knockbackDirection.normalized * _knockbackSpeed;

        if (_animator != null)
        {
            _animator.SetTrigger("Damage");
        }
    }

    private void ReachGoal(Collider goalCollider)
    {
        _state = PlayerState.Finish;
        _isGoal = true;
        _moveInput = Vector2.zero;

        _goalCollider = goalCollider;

        if (_goalCollider != null)
        {
            _goalCollider.enabled = false;
        }
    }

    private void Goal()
    {
        _goalText.text = "Goal!!";

        if (_animator != null)
        {
            _animator.SetFloat("MoveSpeed", 0f);
        }
    }

    private void ResetPlayer()
    {
        _characterController.enabled = false;

        transform.SetPositionAndRotation(
            _startPosition,
            _startRotation
        );

        _characterController.enabled = true;

        _life.Value = _maxHp;
        _verticalVelocity = Vector3.zero;
        _knockbackVelocity = Vector3.zero;
        _moveInput = Vector2.zero;

        _goalText.text = string.Empty;

        _isGoal = false;
        _state = PlayerState.Playing;

        if (_goalCollider != null)
        {
            _goalCollider.enabled = true;
            _goalCollider = null;
        }
    }

    private void Die()
    {
        _state = PlayerState.Finish;
        _moveInput = Vector2.zero;

        if (_animator != null)
        {
            _animator.SetTrigger("Death");
        }
        Destroy(gameObject);
    }
}