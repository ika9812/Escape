using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    public float Speed;
    public float JumpPower;
    public float RotateSpeed;
    public float Gravity;
    public Vector2 Velocity;
}
