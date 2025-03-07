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

    [SerializeField]
    private SpriteRenderer corpseHair;
    [SerializeField]
    private SpriteRenderer corpseShirt;
    [SerializeField]
    private SpriteRenderer corpseSkin;

    private void Start()
    {
        shirt.color = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
        corpseShirt.color = shirt.color;

        hair.color = GetColorCombination(hairColorCorners);
        corpseHair.color = hair.color;

        skin.color = GetColorCombination(skinColorCorners);
        corpseSkin.color = skin.color;
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
