using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject[] enemies;
    public GameObject boss;
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;
    public float spawnRate = 2f;
    private int enemiesSpawned = 0;
    public int maxEnemiesBeforeBoss = 10;

    [Header("Probabilidades de Enemigos")]
    [Range(0, 1)] public float enemy1Probability = 0.3f;
    [Range(0, 1)] public float enemy2Probability = 0.25f;
    [Range(0, 1)] public float enemy3Probability = 0.2f;
    [Range(0, 1)] public float enemy4Probability = 0.15f;
    [Range(0, 1)] public float enemy5Probability = 0.1f;

    private Coroutine spawnCoroutine;

    public void ActivateSpawner()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(10f);
        while (true)
        {
            if (enemiesSpawned < maxEnemiesBeforeBoss)
            {
                Spawn();
            }
            else
            {
                yield return new WaitForSeconds(5f); // Espera antes de aparecer el Boss
                SpawnBoss();
                break;
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    void Spawn()
    {
        int randomEnemyIndex = GetRandomEnemyIndex();
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        if (enemies.Length > 0 && enemies[randomEnemyIndex] != null)
        {
            GameObject spawnedEnemy = Instantiate(enemies[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.Euler(0, 0, 0));
            spawnedEnemy.SetActive(true);

            enemiesSpawned++;
            Debug.Log("Enemigo generado: " + enemies[randomEnemyIndex].name);
        }
        else
        {
            Debug.LogWarning("No se pudo generar el enemigo: " + randomEnemyIndex);
        }
    }

    int GetRandomEnemyIndex()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < enemy1Probability) return 0;
        else if (randomValue < enemy1Probability + enemy2Probability) return 1;
        else if (randomValue < enemy1Probability + enemy2Probability + enemy3Probability) return 2;
        else if (randomValue < enemy1Probability + enemy2Probability + enemy3Probability + enemy4Probability) return 3;
        else return 4;
    }

    void SpawnBoss()
    {
        if (bossSpawnPoint != null)
        {
            Instantiate(boss, bossSpawnPoint.position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            Debug.LogWarning("No se ha asignado un punto de spawn para el Boss.");
        }

        Debug.Log("¡El Boss ha aparecido!");
    }
}
