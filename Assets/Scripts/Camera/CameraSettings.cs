using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [Header("コントローラー操作感度")]
    public float ControllerSensitivity = 1.0f;
    [Header("マウス操作感度")]
    public float MouseSensitivity = 1.0f;
}
