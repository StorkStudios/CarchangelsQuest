using TMPro;
using UnityEngine;

public static class CharacterInfoExtensions
{
    public static void SetCharacterColor(this TMP_TextInfo textInfo, int charIndex, Color32 color)
    {
        if (!textInfo.characterInfo[charIndex].isVisible)
        {
            return;
        }

        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        vertexColors[vertexIndex + 0] = color;
        vertexColors[vertexIndex + 1] = color;
        vertexColors[vertexIndex + 2] = color;
        vertexColors[vertexIndex + 3] = color;

        textInfo.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
