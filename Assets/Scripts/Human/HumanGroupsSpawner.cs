using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HumanGroupsSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject groupPrefab;

    [SerializeField]
    private List<Transform> groupsLocations;

    [SerializeField]
    private float pointSize;

    [SerializeField]
    private Transform groupsParent;

    private IEnumerable<Transform> shuffledGroupPositions;
    private IEnumerator<Transform> nextGroupPosition;

    private bool spawnNextGroup;

    private IEnumerator Start()
    {
        if (groupsLocations == null || groupsLocations.Count == 0)
        {
            yield break;
        }

        shuffledGroupPositions = groupsLocations.Shuffled();
        nextGroupPosition = shuffledGroupPositions.GetEnumerator();
        nextGroupPosition.MoveNext();

        yield return new WaitUntil(() => ScoreKeeper.Instance.HumanKilled > 0);

        while(true)
        {
            SpawnEnemyGroup();
            yield return new WaitUntil(() => spawnNextGroup);
        }
    }

    private void SpawnEnemyGroup()
    {
        spawnNextGroup = false;
        while (Physics2D.OverlapCircle(nextGroupPosition.Current.position, pointSize, LayerMask.GetMask("Wall")) ||
            IsPointVisible(nextGroupPosition.Current.position))
        {
            if (!nextGroupPosition.MoveNext())
            {
                nextGroupPosition = shuffledGroupPositions.GetEnumerator();
                nextGroupPosition.MoveNext();
            }
        }
        GPSMarker marker = Instantiate(groupPrefab, nextGroupPosition.Current.position, Quaternion.Euler(0, 0, Random.Range(0, 359)), groupsParent)
            .GetComponentInChildren<GPSMarker>();
        marker.Destroyed += OnMarkerDestroyed;
        if (!nextGroupPosition.MoveNext())
        {
            nextGroupPosition = shuffledGroupPositions.GetEnumerator();
            nextGroupPosition.MoveNext();
        }
    }

    private void OnMarkerDestroyed(GPSMarker marker)
    {
        marker.Destroyed -= OnMarkerDestroyed;
        spawnNextGroup = true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        foreach (Transform point in groupsLocations)
        {
            Gizmos.DrawWireSphere(point.position, pointSize);
        }
    }

    protected bool IsPointVisible(Vector3 point)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return (new Rect(0, 0, 1, 1)).Contains(viewportPoint);
    }
}
