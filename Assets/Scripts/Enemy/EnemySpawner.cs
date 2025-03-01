using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform enemiesParent;

    [SerializeField]
    private float enemySpawnDelay;

    private HashSet<EnemyController> enemies = new HashSet<EnemyController>();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ScoreKeeper.Instance.HumanKilled > 0);
        while(true)
        {
            
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    private void SpawnEnemy(Vector3 position, float rotation)
    {
        EnemyController enemy = Instantiate(enemyPrefab, position, Quaternion.Euler(0, 0, rotation), enemiesParent).GetComponent<EnemyController>();
        enemies.Add(enemy);
        enemy.Despawned += OnEnemyDespawn;
    }

    private void OnEnemyDespawn(EnemyController enemy)
    {
        enemy.Despawned -= OnEnemyDespawn;
        enemies.Remove(enemy);
    }
}
