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

    [SerializeField]
    private float pointSize;

    private Vector3[] gridCorners = new Vector3[4];


    private void OnValidate()
    {
        gridCorners[0] = gridBoundaries.Min;
        gridCorners[1] = new Vector3(gridBoundaries.Min.x, gridBoundaries.Max.y);
        gridCorners[2] = gridBoundaries.Max;
        gridCorners[3] = new Vector3(gridBoundaries.Max.x, gridBoundaries.Min.y);
    }

#if UNITY_EDITOR
    public void GenerateSpawnPoints()
    {
        spawnPoints.Points = new List<Vector2>();
        for (float x = gridBoundaries.Min.x; x < gridBoundaries.Max.x; x += gridSize)
        {
            for (float y = gridBoundaries.Min.y; y < gridBoundaries.Max.y; y += gridSize)
            {
                float v = Mathf.PerlinNoise(x / noiseScale, y / noiseScale);
                Vector2 position = new Vector2(x, y);
                if (v > noiseThreshold && !Physics2D.OverlapCircle(position, pointSize, LayerMask.GetMask("Wall")))
                {
                    spawnPoints.Points.Add(position);
                }
            }
        }
        EditorUtility.SetDirty(spawnPoints);
        AssetDatabase.SaveAssets();
    }
#endif

    protected virtual void OnDrawGizmosSelected()
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
                Gizmos.DrawWireSphere(new Vector3(x, y, 0), pointSize);
            }
        }
        Gizmos.color = Color.green;
        foreach (Vector2 point in spawnPoints.Points)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x, point.y, 0), pointSize);
        }
    }

    protected bool IsPointVisible(Vector3 point)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return (new Rect(0, 0, 1, 1)).Contains(viewportPoint);
    }
}
