using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : SpawnerBase
{
    [SerializeField]
    private GameObject humanPrefab;

    [SerializeField]
    private float humanSpawnDelay;

    [SerializeField]
    private float startHumansCount;

    [SerializeField]
    private Transform humansParent;

    private IEnumerable<Vector2> shuffledHumanPositions;
    private IEnumerator<Vector2> nextHumanPosition;

    private IEnumerator Start()
    {
        if (spawnPoints == null || spawnPoints.Points == null || spawnPoints.Points.Count == 0)
        {
            Debug.LogError("Human spawn points not generated. Humans will not be spawned");
            yield break;
        }
        
        shuffledHumanPositions = spawnPoints.Points.Shuffled();
        nextHumanPosition = shuffledHumanPositions.GetEnumerator();
        nextHumanPosition.MoveNext();

        for (int i = 0; i < startHumansCount; i++)
        {
            SpawnHuman(false);
        }

        while (true)
        {
            yield return new WaitForSeconds(humanSpawnDelay);
            SpawnHuman();
        }
    }

    private void SpawnHuman(bool notInVisionRange = true)
    {
        while (Physics2D.OverlapCircle(nextHumanPosition.Current, 0.2f, LayerMask.GetMask("Wall")) ||
            (notInVisionRange && IsPointVisible(nextHumanPosition.Current)))
        {
            if (!nextHumanPosition.MoveNext())
            {
                nextHumanPosition = shuffledHumanPositions.GetEnumerator();
                nextHumanPosition.MoveNext();
            }
        }
        Instantiate(humanPrefab, nextHumanPosition.Current, Quaternion.Euler(0, 0, Random.Range(0, 359)), humansParent);
        if (!nextHumanPosition.MoveNext())
        {
            nextHumanPosition = shuffledHumanPositions.GetEnumerator();
            nextHumanPosition.MoveNext();
        }
    }
}
