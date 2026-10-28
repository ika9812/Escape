using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamera : MonoBehaviour
{
    [SerializeField] PlayerInput playerinput;

    [SerializeField] Transform looktarget;

    [SerializeField] Vector3 offset;

    [SerializeField] float distance;

    [SerializeField] float rotatesp;

    float pitch;
    float yaw;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yaw = 90.0f;
        pitch = 0.0f;  
    }

    // Update is called once per frame
    void Update()
    {
        var lookvec = playerinput.actions["Look"].ReadValue<Vector2>();
        yaw += lookvec.x * rotatesp * Time.deltaTime;
        pitch -= lookvec.y * rotatesp * Time.deltaTime;

        var target = looktarget.position + offset;
        var rotaition=Quaternion.Euler(pitch, yaw, 0);
        var posisiton = rotaition * new Vector3(0, 0, -distance) + target;

        transform.rotation = rotaition;
        transform.position = posisiton;
    }
}
