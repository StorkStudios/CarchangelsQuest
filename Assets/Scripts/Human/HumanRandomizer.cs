using System.Collections.Generic;
using UnityEngine;

public class HumanRandomizer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private List<Color> skinColorCorners;

    [SerializeField]
    private List<Color> hairColorCorners;

    [Header("References")]
    [SerializeField]
    private SpriteRenderer hair;

    [SerializeField]
    private SpriteRenderer shirt;

    [SerializeField]
    private SpriteRenderer skin;

    private void Start()
    {
        shirt.color = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
        hair.color = GetColorCombination(hairColorCorners);
        skin.color = GetColorCombination(skinColorCorners);
    }

    private Color GetColorCombination(List<Color> colors)
    {
        float[] coeficients = new float[colors.Count];
        float coeficientsSum = 0;
        for (int i = 0; i < colors.Count; i++)
        {
            coeficients[i] = Random.value;
            coeficientsSum += coeficients[i];
        }
        Color color = new Color();
        for (int i = 0; i < colors.Count; i++)
        {
            color += colors[i] * (coeficients[i] / coeficientsSum);
        }
        return color;
    }
}
