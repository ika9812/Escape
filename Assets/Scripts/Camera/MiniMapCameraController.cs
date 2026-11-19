
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField] private Transform Player;

    [SerializeField]private float Height = 30f;

    //[SerializeField] private RectTransform PlayerIcon;

    private void LateUpdate()
    {
        if (Player == null) return;

        Vector3 position = Player.position;

        transform.position = new Vector3(
            position.x,
            position.y + Height,
            position.z
        );

        //float rotation = -Player.eulerAngles.y;

        //PlayerIcon.localRotation =
        //    Quaternion.Euler(0f, 0f, rotation);
    }
}
