using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    CameraInputHandle CameraInputHandle;
    CameraController CameraController;


    PlayerInput PlayerInput;
    [SerializeField] CameraSettings CameraSettings;
    [SerializeField, Tooltip("カメラが注視するターゲット")] private Transform LookTarget;
    [SerializeField, Tooltip("カメラのオフセット")] private Vector3 OffSet;
    [SerializeField, Tooltip("カメラとターゲットの距離")] private float Distance;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        CameraInputHandle = new CameraInputHandle(PlayerInput);
        CameraController = new CameraController(LookTarget, OffSet, Distance, CameraSettings);
    }

    void OnEnable()
    {
        CameraInputHandle.CameraInputEnable();
    }

    void OnDisable()
    {
        CameraInputHandle.CameraInputDisable();
    }

    void LateUpdate()
    {
        bool usingMouse = PlayerInput.actions["Look"].activeControl?.device is Mouse;

        CameraController.SetCameraDistance(CameraInputHandle.ZoomInput.y);

        if (usingMouse)
        {
            CameraController.UpdateMouseCamera(CameraInputHandle.CameraInput);
        }
        else
        {
            CameraController.UpdateControllerCamera(CameraInputHandle.CameraInput);
        }
        CameraController.UpdateCamera(transform);

    }
}
