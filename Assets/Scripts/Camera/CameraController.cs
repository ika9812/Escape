using UnityEngine;

public class CameraController
{
    CameraSettings CameraSettings;
    Transform LookTarget;// カメラが注視するターゲット
    Vector3 OffSet;// カメラの注視点
    float Distance;// カメラと注視点の距離
    float MinPitch = 0;// カメラの上下回転の最小角度
    float MaxPitch = 80;// カメラの上下回転の最大角度
    float Pitch = 0;// カメラの上下回転角度
    float Yaw = 90;// カメラの左右回転角度

    public CameraController(Transform lookTarget, Vector3 offSet, float distance, CameraSettings cameraSettings)
    {
        LookTarget = lookTarget;
        OffSet = offSet;
        Distance = distance;
        CameraSettings = cameraSettings;
    }

    public void UpdateMouseCamera(Vector2 lookVector)
    {
        Yaw += lookVector.x * CameraSettings.MouseSensitivity;
        Pitch -= lookVector.y * CameraSettings.MouseSensitivity;

    }
    public void UpdateControllerCamera(Vector2 lookVector)
    {
        Yaw += lookVector.x * CameraSettings.ControllerSensitivity;
        Pitch -= lookVector.y * CameraSettings.ControllerSensitivity;
    }

    public void UpdateCamera(Transform transform)
    {
        Pitch = Mathf.Clamp(Pitch, MinPitch, MaxPitch);

        Vector3 targetposition = LookTarget.position + OffSet;

        Quaternion rotation = Quaternion.Euler(Pitch, Yaw, 0);

        Vector3 cameraPosition = rotation * new Vector3(0, 0, -Distance) + targetposition;

        transform.SetPositionAndRotation(cameraPosition, rotation);
    }

    public void SetCameraDistance(float scroll)
    {
        Distance -= scroll;

        Distance = Mathf.Clamp(Distance, 3f, 5f);
    }
}
