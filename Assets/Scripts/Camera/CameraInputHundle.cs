using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandle
{
    PlayerInput PlayerInput;

    public Vector2 CameraInput { get; private set; }
    public Vector2 ZoomInput { get; private set; }

    public CameraInputHandle(PlayerInput playerInput)
    {
        PlayerInput = playerInput;
    }
    public void CameraInputEnable()
    {
        PlayerInput.actions["Look"].performed += OnLook;
        PlayerInput.actions["Look"].canceled += OnLook;
        //PlayerInput.actions["Zoom"].performed += OnZoom;
        //PlayerInput.actions["Zoom"].canceled += OnZoom;
    }
    public void CameraInputDisable()
    {
        PlayerInput.actions["Look"].performed -= OnLook;
        PlayerInput.actions["Look"].canceled -= OnLook;
        //PlayerInput.actions["Zoom"].performed -= OnZoom;
        //PlayerInput.actions["Zoom"].canceled -= OnZoom;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        CameraInput = context.ReadValue<Vector2>();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        ZoomInput = context.ReadValue<Vector2>();
    }
}
