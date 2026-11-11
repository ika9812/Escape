using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountDownText;

    [SerializeField] private TextMeshProUGUI _goalText;

    public int _maxHp = 100;
    public ReactiveProperty<int> _life { get; private set; } = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountDownText.text = string.Empty;
        _goalText.text = string.Empty;
        _life.Value = _maxHp;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async UniTask StartCountDownAsync()
    {
        await UniTask.Delay(
            1000,
            cancellationToken: destroyCancellationToken
        );

        string[] countTexts = { "3", "2", "1", "Go!" };

        foreach (string text in countTexts)
        {
            CountDownText.text = text;

            await UniTask.Delay(
                1000,
                cancellationToken: destroyCancellationToken
            );
        }

        CountDownText.text = string.Empty;
    }

    private void Goal()
    {
        _goalText.text = "Goal!!";
    }
}
