using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : SpawnerBase
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform enemiesParent;

    [SerializeField]
    private float enemySpawnDelay;

    [SerializeField]
    private float enemySpawnRange;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float optimalEnemySpawnAngle;

    private HashSet<EnemyController> enemies = new HashSet<EnemyController>();

    private IEnumerator Start()
    {
        GameObject spawnTrigger = new GameObject("EnemySpawnPoint");
        spawnTrigger.transform.parent = enemiesParent;
        CircleCollider2D collider = spawnTrigger.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 1;
        spawnTrigger.layer = LayerMask.NameToLayer("EnemySpawnPoints");
        foreach (Vector2 point in spawnPoints.Points)
        {
            Instantiate(collider, point, Quaternion.identity, enemiesParent);
        }

        yield return new WaitUntil(() => ScoreKeeper.Instance.HumanKilled > 0);

        while(true)
        {
            Vector3 spawnpoint = GetEnemySpawnPoint();
            Debug.Log($"Spawning enemy at {spawnpoint}");
            SpawnEnemy(spawnpoint, Vector2.Angle(Vector2.up, player.position - spawnpoint));
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    private Vector3 GetEnemySpawnPoint()
    {
        HashSet<Collider2D> collidersInRange = Physics2D.OverlapCircleAll(player.position, enemySpawnRange, LayerMask.GetMask("EnemySpawnPoints")).ToHashSet();
        var corners = (Camera.main.ViewportToWorldPoint(Vector3.zero), Camera.main.ViewportToWorldPoint(Vector3.one));
        HashSet<Collider2D> collidersOnScreen = Physics2D.OverlapAreaAll(corners.Item1, corners.Item2, LayerMask.GetMask("EnemySpawnPoints")).ToHashSet();
        collidersInRange.ExceptWith(collidersOnScreen);
        if (collidersInRange.Count == 0)
        {
            Debug.LogError("No spawnpoint for enemy found. Spawn range should be bigger");
        }
        return collidersInRange.OrderBy(c => Mathf.Abs(Vector2.Angle(player.up, c.transform.position - player.position) - optimalEnemySpawnAngle)).First().transform.position;
    }

    private void SpawnEnemy(Vector3 position, float rotation)
    {
        EnemyController enemy = Instantiate(enemyPrefab, position, Quaternion.Euler(0, 0, rotation), enemiesParent).GetComponent<EnemyController>();
        enemy.Player = player;
        enemies.Add(enemy);
        enemy.Despawned += OnEnemyDespawn;
    }

    private void OnEnemyDespawn(EnemyController enemy)
    {
        enemy.Despawned -= OnEnemyDespawn;
        enemies.Remove(enemy);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, enemySpawnRange);
    }
}
