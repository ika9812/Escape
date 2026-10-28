using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using R3.Triggers;

public class HpGauge :MonoBehaviour
{
    [SerializeField] Slider gaugeImage;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI maxLifeText;
    public void SetMaxLifeText(float v)
    {
        maxLifeText.text = v.ToString();
    }
    public void SetLifeText(float v)
    {
        lifeText.text = v.ToString();
    }
    public void SetLifeGauge(float t)
    {
        gaugeImage.value= t;
    }
}
