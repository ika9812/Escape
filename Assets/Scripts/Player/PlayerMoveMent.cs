using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMoveMent 
{
    private CharacterController CharacterController;

    PlayerInput PlayerInput;

    PlayerInputHandle Input;

    PlayerStatus Status;

    private PlayerMoveMent(CharacterController characterController,PlayerInput playerInput,PlayerInputHandle playerInputHandle,PlayerStatus playerStatus)
    {
        CharacterController = characterController;
        PlayerInput = playerInput;
        Input = playerInputHandle;
        Status = playerStatus;
    }


    private void Move(Transform transform)
    {
        Camera inputCamera = PlayerInput.GetComponent<Camera>();

        if (inputCamera == null)
            return;

        Vector3 cameraForward = inputCamera.transform.forward;
        Vector3 cameraRight = inputCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction =
            cameraRight * Input.MoveInput.x +
            cameraForward * Input.MoveInput.y;

        direction = Vector3.ClampMagnitude(direction, 1f);

        CharacterController.Move(direction * Status.Speed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Status.RotateSpeed * Time.deltaTime
            );
        }
    }

    public void Jump()
    {
        Status.Velocity.y = Mathf.Sqrt(Status.JumpPower * -2f * Status.Gravity);
    }

    public bool IsGround()
    {
        return CharacterController.isGrounded;
    }

    public void Gravity()
    {
        if (CharacterController.isGrounded && Status.Velocity.y < 0)
        {
            Status.Velocity.y = -2f;

        }

        Status.Velocity.y += Status.Gravity * Time.deltaTime;
        CharacterController.Move(Status.Velocity * Time.deltaTime);
    }
}
