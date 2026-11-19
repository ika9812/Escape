using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public PlayerInputHandle Input { get; private set; }
    public PlayerMoveMent Move { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }


    [SerializeField] private PlayerStatus Status;

    CharacterController CharacterController;
    PlayerInput PlayerInput;

    [SerializeField] private Animator Animator;
  


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        PlayerInput = GetComponent<PlayerInput>();

        //Status = new PlayerStatus();
        Input = new PlayerInputHandle(PlayerInput);
        Move = new PlayerMoveMent(CharacterController, PlayerInput, Input, Status);
        StateMachine = new PlayerStateMachine();
    }

    private void OnEnable()
    {
        Input.EnableInput();
    }

    private void OnDisable()
    {
        Input.DisableInput();
    }

    private void Start()
    {
        StateMachine.ChangeState(new IdleState(this));
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if(!GamaManager.Instance.IsPlaying)
        {
            return;
        }
        StateMachine.Update();
        Move.Gravity();
    }

    
}