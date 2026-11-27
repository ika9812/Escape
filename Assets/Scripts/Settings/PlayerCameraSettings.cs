using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCameraSettings", menuName = "Scriptable Objects/PlayerCameraSettings")]
public class PlayerCameraSettings : ScriptableObject
{
    [Header("コントローラー操作感度"),Range(0,10)]
    public float ControllerSensitivity = 1.0f;
    [Header("マウス操作感度"),Range(0,1)]
    public float MouseSensitivity = 1.0f;

}
