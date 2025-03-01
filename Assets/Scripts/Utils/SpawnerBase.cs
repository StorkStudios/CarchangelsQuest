using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [Header("Points generation")]
    [SerializeField]
    [EditObjectInInspector]
    protected SpawnPoints spawnPoints;

    [SerializeField]
    private RangeBoundaries<Vector2> gridBoundaries;

    [SerializeField]
    private float noiseThreshold;

    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private float gridSize;

    private Vector3[] gridCorners = new Vector3[4];

    private void OnValidate()
    {
        gridCorners[0] = gridBoundaries.Min;
        gridCorners[1] = new Vector3(gridBoundaries.Min.x, gridBoundaries.Max.y);
        gridCorners[2] = gridBoundaries.Max;
        gridCorners[3] = new Vector3(gridBoundaries.Max.x, gridBoundaries.Min.y);
    }

    public void GenerateSpawnPoints()
    {
        spawnPoints.Points = new List<Vector2>();
        for (float x = gridBoundaries.Min.x; x < gridBoundaries.Max.x; x += gridSize)
        {
            for (float y = gridBoundaries.Min.y; y < gridBoundaries.Max.y; y += gridSize)
            {
                float v = Mathf.PerlinNoise(x / noiseScale, y / noiseScale);
                Vector2 position = new Vector2(x, y);
                if (v > noiseThreshold && !Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Wall")))
                {
                    spawnPoints.Points.Add(position);
                }
            }
        }
        EditorUtility.SetDirty(spawnPoints);
        AssetDatabase.SaveAssets();
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLineStrip(gridCorners, true);
        for (float x = gridBoundaries.Min.x; x < gridBoundaries.Max.x; x += gridSize)
        {
            for (float y = gridBoundaries.Min.y; y < gridBoundaries.Max.y; y += gridSize)
            {
                Gizmos.DrawWireSphere(new Vector3(x, y, 0), 0.2f);
            }
        }
        Gizmos.color = Color.green;
        foreach (Vector2 point in spawnPoints.Points)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x, point.y, 0), 0.2f);
        }
    }

    protected bool IsPointVisible(Vector3 point)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return (new Rect(0, 0, 1, 1)).Contains(viewportPoint);
    }
}
