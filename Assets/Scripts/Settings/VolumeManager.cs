using Assets.Scripts.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// 音量調整用UIにアタッチするクラス
/// </summary>
public class VolumeManager : MonoBehaviour
{
    [Header("音量調整用のslider")]
    [SerializeField,Header("全体音量")] private Slider MasterSlider;

    [SerializeField, Header("BGM音量")] private Slider BGMSlider;

    [SerializeField, Header("SE音量")] private Slider SESlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MasterSlider.value = AudioManager.Instance.GetMasterVolume();
        BGMSlider.value = AudioManager.Instance.GetBGMVolume();
        SESlider.value = AudioManager.Instance.GetSEVolume();

        MasterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        BGMSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        SESlider.onValueChanged.AddListener(AudioManager.Instance.SetSEVolume);
    }



}
