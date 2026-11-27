using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// コントローラーとキーボードでのタイトルシーン制御,設定以外のパネルのオンオフ
/// </summary>
public class TitleManager : MonoBehaviour
{
    PlayerInput PlayerInput;

    [SerializeField]private TMP_Text TitleText;

    [SerializeField]float MinScale;
    [SerializeField]float MaxScale;
    [SerializeField]float Duration;

    private Tween ScaleTween;

    [SerializeField]Button StartButton;
    [SerializeField]Button EndButton;

    private void OnEnable()
    {
        StartButton.onClick.AddListener(GameStart);
        EndButton.onClick.AddListener(GameEnd);
    }

    private void OnDisable()
    {
        StartButton.onClick.RemoveListener(GameStart);
        EndButton.onClick.RemoveListener(GameEnd);
    }

    private void Start()
    {
        ScaleTween = TitleText.transform.DOScale(MaxScale, Duration)
            .From(MinScale)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void GameStart()
    {
        SceneManager.LoadScene("Main");
    }

    void GameEnd()
    {
        Application.Quit();
    }
}
