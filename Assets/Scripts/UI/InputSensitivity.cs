using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// マウスとコントローラーの入力感度変更用
/// </summary>
public class InputSensitivity : MonoBehaviour
{
    [SerializeField]private Slider MouseSlider;
    [SerializeField]private Slider ControllerSlider;

    //----------入力感度を数字を直接入力して変更するため(実装予定)---------
    //[SerializeField]private InputField MouseField;
    //[SerializeField]private InputField ControllerField;
    //---------------------------------------------------------------//

    [SerializeField]private PlayerCameraSettings InputSens;
    //getで値を取得
    //setで値の更新
    private void Start()
    {
        MouseSlider.value = GetMouseSens();
        ControllerSlider.value = GetControllerSens();

        MouseSlider.onValueChanged.AddListener(SetMouseSens);
        ControllerSlider.onValueChanged.AddListener(SetControllerSens);
    }

    private float GetMouseSens()
    {
        return InputSens.MouseSensitivity;
    }

    private float GetControllerSens()
    {
        return InputSens.ControllerSensitivity;
    }

    private void SetMouseSens(float value)
    {
        InputSens.MouseSensitivity = value;
    }

    private void SetControllerSens(float value)
    {
        InputSens.ControllerSensitivity = value * 10;
    }
}
