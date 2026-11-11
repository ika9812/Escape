using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;

    [SerializeField] Transform LookTarget;

    [SerializeField] Vector3 Offset;

    [SerializeField] float Distance;

    [SerializeField] float RotateSpeed;

    float Pitch;
    float Yaw;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Yaw = 90.0f;
        Pitch = 0.0f;  
    }

    // Update is called once per frame
    void Update()
    {
        var lookvec = PlayerInput.actions["Look"].ReadValue<Vector2>();
        Yaw += lookvec.x * RotateSpeed * Time.deltaTime;
        Pitch -= lookvec.y * RotateSpeed * Time.deltaTime;

        var target = LookTarget.position + Offset;
        var rotaition=Quaternion.Euler(Pitch, Yaw, 0);
        var posisiton = rotaition * new Vector3(0, 0, -Distance) + target;

        transform.rotation = rotaition;
        transform.position = posisiton;
    }
}
