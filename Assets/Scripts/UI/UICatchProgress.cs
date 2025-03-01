using DG.Tweening;
using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UICatchProgress : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI frontText;
    [SerializeField]
    private TextMeshProUGUI backText;
    [SerializeField]
    private float fadeDuration;
    [SerializeField]
    private Gradient gradient;

    private CanvasGroup canvasGroup;
    private PlayerLoser playerLoser;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        playerLoser = FindAnyObjectByType<PlayerLoser>();
        playerLoser.CatchProgessChanged += OnCatchProgessChanged;
        frontText.OnPreRenderText += OnPreRenderFrontText;
    }

    private void OnCatchProgessChanged(float oldValue, float newValue)
    {
        if (oldValue <= 0 && newValue > 0)
        {
            Fade(true);
        }
        else if (newValue <= 0)
        {
            Fade(false);
        }

        int number = (int)(backText.text.Length * newValue);
        frontText.text = new string('-', number);
    }

    private void OnPreRenderFrontText(TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            Color color = gradient.Evaluate((float)i / backText.text.Length);
            textInfo.SetCharacterColor(i, color);
        }
    }

    private void Fade(bool fadeIn)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(fadeIn ? 1 : 0, fadeDuration);
    }
}
