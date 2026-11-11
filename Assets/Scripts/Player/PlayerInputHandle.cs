using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandle 
{
    private PlayerInput PlayerInput;

    public Vector2 MoveInput { get; private set; }

    public bool IsJump { get; private set; }

    public PlayerInputHandle(PlayerInput playerInput)
    {
        PlayerInput = playerInput;
    }

    public void EnableInput()
    {
        PlayerInput.actions["Move"].performed += OnMove;
        PlayerInput.actions["Move"].canceled += OnMove;
        PlayerInput.actions["Jump"].performed += OnJump;
    }

    public void DisableInput()
    {
        PlayerInput.actions["Move"].performed -= OnMove;
        PlayerInput.actions["Move"].canceled -= OnMove;
        PlayerInput.actions["Jump"].performed -= OnJump;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        IsJump = true;
    }
}
