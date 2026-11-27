using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 設定パネルのタブとボタンの構造体
/// </summary>
[System.Serializable]
struct SettingTab
{
    public GameObject Tab;
    public Button Button;
}

/// <summary>
/// 設定 UI の表示と管理を提供する MonoBehaviour コンポーネント。
/// </summary>
public class SettingsUIManager : MonoBehaviour
{
    [SerializeField]SettingTab[] SettingTabs;

    [SerializeField,Header("設定パネルの表示用ボタン")]Button SettingTrue;
    [SerializeField,Header("設定パネルの非表示用ボタン")]Button SettingFalse;

    [SerializeField,Header("設定パネル")] GameObject SettingPanel;

    private void Start()
    {
        SettingPanel.SetActive(false);   
    }

    private void OnEnable()
    {
        for(int i= 0;i<SettingTabs.Length;i++)
        {
            int index = i;
            SettingTabs[index].Button.onClick.AddListener(() => ChangeSettingsTab(index));
        }
        SettingTrue.onClick.AddListener(() => SettingPanel.SetActive(true));
        SettingFalse.onClick.AddListener(()=>SettingPanel.SetActive(false));
    }
    private void OnDisable()
    {
        for (int i = 0; i < SettingTabs.Length; i++)
        {
            int index = i;
            SettingTabs[index].Button.onClick.RemoveListener(() => ChangeSettingsTab(index));
        }
        
        SettingTrue.onClick.RemoveListener(()=> SettingPanel.SetActive(true));
        SettingFalse.onClick.RemoveListener(()=> SettingPanel.SetActive(false));
    }

    /// <summary>
    /// 設定項目の切り替え
    /// </summary>
    public void ChangeSettingsTab(int index)
    {
        for (int i = 0; i < SettingTabs.Length; i++)
        {
            if (i == index)
            {
                SettingTabs[i].Tab.SetActive(true);
            }
            else
            {
                SettingTabs[i].Tab.SetActive(false);
            }
        }
    }

}
