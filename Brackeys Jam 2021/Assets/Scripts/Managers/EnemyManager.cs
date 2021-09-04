using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> currentEnemies = new List<Enemy>();
    public List<Spawner> currentSpawners = new List<Spawner>();

    public GameObject[] enemies;
    public float spawnDelay = 3.0f;
    public int maxEnemiesAtOnce = 10;

    void Start() {
        StartCoroutine("SpawnEnemiesPeriodically");
    }

    public void SpawnEnemyAtRandomSpawner() {

        //don't spawn too many enemies at once for fear of overcrowding the screen and lag
        if (currentEnemies.Count >= maxEnemiesAtOnce) return;

        GameObject enemyToSpawn = enemies[Mathf.FloorToInt(Random.Range(0, enemies.Length))];
        Vector3 locationToSpawnAt = currentSpawners[Mathf.FloorToInt(Random.Range(0, currentSpawners.Count))].gameObject.transform.position;

        Instantiate(enemyToSpawn, locationToSpawnAt, Quaternion.identity);

    }

    IEnumerator SpawnEnemiesPeriodically() {
        while (true) {
            yield return new WaitForSeconds(spawnDelay);
            if (currentSpawners.Count > 0) {
                SpawnEnemyAtRandomSpawner();
                spawnDelay = Random.Range(3.0f, 10.0f);
            }
        }
    }
}
