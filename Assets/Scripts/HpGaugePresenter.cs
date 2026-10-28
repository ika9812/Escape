using UnityEngine;
using R3;
public class HpGaugePresenter : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] HpGauge _hpGauge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hpGauge.SetMaxLifeText(_player._maxHp);

        _player._life.Subscribe(l => _hpGauge.SetLifeText(l)).AddTo(this);

        _player._life.Select(l => l / _player._maxHp).Subscribe(l => _hpGauge.SetLifeGauge(l)).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
