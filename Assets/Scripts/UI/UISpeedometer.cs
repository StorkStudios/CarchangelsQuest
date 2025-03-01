using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedometer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI frontText;
    [SerializeField]
    private TextMeshProUGUI backText;
    [SerializeField]
    private float speedToKPHConvertValue;
    [SerializeField]
    private string suffix;
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private Image barFill;

    private CarEngine engine;

    private void Start()
    {
        engine = FindObjectsByType<CarEngine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).First(e => e.CompareTag(Tags.Player));
    }

    private void LateUpdate()
    {
        float speed = engine.ForwardSpeed;
        int shownSpeed = (int)(speed * speedToKPHConvertValue);
        SetText($"{shownSpeed}{suffix}");
        float t = speed / engine.MaxSpeed;
        transform.localScale = Vector3.one * Mathf.LerpUnclamped(1, maxScale, t);
        barFill.fillAmount = Mathf.Clamp01(t);
    }

    private void SetText(string text)
    {
        frontText.text = text;
        backText.text = text;
    }
}
