using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject humanPrefab;

    [SerializeField]
    private float humanSpawnDelay;

    [Header("Points generation")]
    [SerializeField]
    [EditObjectInInspector]
    private HumanSpawnPoints spawnPoints;

    [SerializeField]
    private RangeBoundaries<Vector2> gridBoundaries;

    [SerializeField]
    private float noiseThreshold;

    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private float gridSize;

    private Vector3[] gridCorners = new Vector3[4];
    private int navmeshAreaMask;

    private IEnumerator Start()
    {
        navmeshAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        while (true)
        {
            yield return new WaitForSeconds(humanSpawnDelay);
        }
    }

    private void OnValidate()
    {
        gridCorners[0] = gridBoundaries.Min;
        gridCorners[1] = new Vector3(gridBoundaries.Min.x, gridBoundaries.Max.y);
        gridCorners[2] = gridBoundaries.Max;
        gridCorners[3] = new Vector3(gridBoundaries.Max.x, gridBoundaries.Min.y);
    }

    public void GenerateSpawnPoints()
    {
        spawnPoints.SpawnPoints.Clear();
        for (float x = gridBoundaries.Min.x; x < gridBoundaries.Max.x; x += gridSize)
        {
            for (float y = gridBoundaries.Min.y; y < gridBoundaries.Max.y; y += gridSize)
            {
                float v = Mathf.PerlinNoise(x / noiseScale, y / noiseScale);
                Vector2 position = new Vector2(x, y);
                if (v > noiseThreshold && !Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Wall")))
                {
                    spawnPoints.SpawnPoints.Add(position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
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
        foreach (Vector2 point in spawnPoints.SpawnPoints)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x, point.y, 0), 0.2f);
        }
    }
}
