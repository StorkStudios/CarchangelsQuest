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

    public static void SetCharacterOffset(this TMP_TextInfo textInfo, int charIndex, Vector2 offset, Vector3[] originalVertices)
    {
        if (!textInfo.characterInfo[charIndex].isVisible)
        {
            return;
        }

        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        for (int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = originalVertices[i] + offset.ToVector3();
        }

        textInfo.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    public static void SetCharacterSizeScale(this TMP_TextInfo textInfo, int charIndex, float scale, Vector3[] originalVertices)
    {
        if (!textInfo.characterInfo[charIndex].isVisible)
        {
            return;
        }

        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        Vector3 midPoint = (originalVertices[0] + originalVertices[1] + originalVertices[2] + originalVertices[3]) / 4;

        for (int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = Vector3.LerpUnclamped(midPoint, originalVertices[i], scale);
        }

        textInfo.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
