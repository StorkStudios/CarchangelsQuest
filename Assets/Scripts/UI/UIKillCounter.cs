using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIKillCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI frontText;
    [SerializeField]
    private TextMeshProUGUI backText;
    [SerializeField]
    private float animationInitialScale;
    [SerializeField]
    private float animationDuration;

    private void Start()
    {
        UpdateKillCount(ScoreKeeper.Instance.HumanKilled);
        ScoreKeeper.Instance.humansKilledChanged += OnHumanKillsChanged;
    }

    private void OnHumanKillsChanged(int oldValue, int newValue)
    {
        UpdateKillCount(newValue);
    }

    private void UpdateKillCount(int count)
    {
        SetText(count.ToString());
        SetColorHue((count % 255) / 255f);
        RestartAnimation();
    }

    private void RestartAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * animationInitialScale;
        transform.DOScale(1, animationDuration);
    }

    private void SetText(string text)
    {
        frontText.text = text;
        backText.text = text;
    }

    private void SetColorHue(float hue)
    {
        frontText.color = Color.HSVToRGB(hue, 1, 1);
    }
}
