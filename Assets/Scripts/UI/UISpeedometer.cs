using System.Linq;
using TMPro;
using UnityEngine;

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
    private float maxSpeed;
    [SerializeField]
    private float maxScale;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = FindObjectsByType<Rigidbody2D>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).First(e => e.CompareTag(Tags.Player));
    }

    private void LateUpdate()
    {
        float speed = rigidbody.linearVelocity.magnitude;
        int shownSpeed = (int)(speed * speedToKPHConvertValue);
        SetText($"{shownSpeed}{suffix}");
        transform.localScale = Vector3.one * Mathf.LerpUnclamped(1, maxScale, speed / maxSpeed);
    }

    private void SetText(string text)
    {
        frontText.text = text;
        backText.text = text;
    }
}
