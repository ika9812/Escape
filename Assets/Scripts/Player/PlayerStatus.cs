using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    public float Speed = 5f;
    public float JumpPower = 8f;
    public float RotateSpeed = 10f;
    public float Gravity = -20f;
    public Vector2 Velocity;
}
