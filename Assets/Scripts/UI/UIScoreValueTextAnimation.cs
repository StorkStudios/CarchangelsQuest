using System.Collections;
using TMPro;
using UnityEngine;

public class UIScoreValueTextAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI frontText;
    [SerializeField]
    private float animationDuration;
    [SerializeField]
    private float letterDelay;
    [SerializeField]
    private Gradient gradient;

    private float startTimestamp;

    private void Start()
    {
        startTimestamp = Time.time;
    }

    private void Update()
    {
        frontText.ForceMeshUpdate();
        for (int i = 0; i < frontText.textInfo.characterCount; i++)
        {
            float time = Time.time - startTimestamp;
            time -= i * letterDelay;
            if (time <= 0)
            {
                continue;
            }

            time = time % animationDuration;
            float t = time / animationDuration;

            frontText.textInfo.SetCharacterColor(i, gradient.Evaluate(t));
        }
    }
}
