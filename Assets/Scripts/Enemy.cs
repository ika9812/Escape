using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("移動")]
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _rotateSpeed = 3f;

    [Header("ステータス")]
    [SerializeField] int _hp = 3;

    [Header("被弾")]
    [SerializeField] float _invincibleTimeMax = 1.5f;
    [SerializeField] float _knockbackSpeed = 5f;

    public Collider _playerCollider { get; set; }

    Rigidbody _rb;

    float _invincibleTime;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (_playerCollider == null) return;

        UpdateInvincibleTime();

        if (_invincibleTime > 0) return;

        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            StopMove();
        }
    }


    void UpdateInvincibleTime()
    {
        if (_invincibleTime > 0)
        {
            _invincibleTime -= Time.deltaTime;
        }
    }


    bool CanSeePlayer()
    {
        Vector3 start = _rb.position;
        Vector3 target = _playerCollider.bounds.center;

        Vector3 direction = target - start;

        float distance = direction.magnitude;

        if (Physics.Raycast(start,direction.normalized,out RaycastHit hit,distance))
        {
            return hit.collider != _playerCollider;
        }

        return false;
    }


    void ChasePlayer()
    {
        Vector3 direction =
            _playerCollider.bounds.center - _rb.position;

        direction.y = 0;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Vector3 moveDirection = direction.normalized;

        Vector3 velocity =
            moveDirection * _moveSpeed;

        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;

        transform.forward = Vector3.Slerp(
            transform.forward,
            moveDirection,
            _rotateSpeed * Time.deltaTime
        );
    }


    void StopMove()
    {
        _rb.linearVelocity = new Vector3(
            0,
            _rb.linearVelocity.y,
            0
        );
    }


    private void OnCollisionStay(Collision collision)
    {
        if (_invincibleTime > 0)
            return;

        AttackObject attackObj =
            collision.gameObject.GetComponent<AttackObject>();

        if (attackObj == null)
            return;

        if(collision.gameObject.CompareTag("fire"))
        {
            TakeDamage(
            attackObj.power,
            collision.transform.position
        );
        }
        
    }


    void TakeDamage(int damage, Vector3 attackPosition)
    {
        _hp -= damage;

        _invincibleTime =
            _invincibleTimeMax;

        Vector3 direction =
            transform.position - attackPosition;

        direction.y = 0;

        Vector3 knockback =
            direction.normalized *
            _knockbackSpeed;

        _rb.linearVelocity = knockback;


        if (_hp <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }
}