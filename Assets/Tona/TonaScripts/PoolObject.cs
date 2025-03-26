using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float spawnXStart ; // Posición base en X
    [SerializeField] private float spawnXRange; // Variación en X
    [SerializeField] private float spawnZStart ; // Posición base en Z
    [SerializeField] private float spawnZRange; // Variación en Z
  
    private readonly List<GameObject> pool = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPosition = GetSpawnPositionWithSpacing();
            GameObject cactus = Instantiate(Prefab, spawnPosition, Quaternion.identity);
            cactus.SetActive(true);
            pool.Add(cactus);
        }
    }

    public GameObject Getcactus()
    {
        GameObject cactus = pool.Find(c => !c.activeInHierarchy);
        if (cactus == null)
        {
            cactus = Instantiate(Prefab);
            pool.Add(cactus);
        }

        cactus.transform.position = GetSpawnPositionWithSpacing();
        cactus.SetActive(true);
        return cactus;
    }

    public void Returncactus(GameObject cactus)
    {
        cactus.SetActive(true);
        cactus.transform.position = GetSpawnPositionWithSpacing();
    }

    private Vector3 GetSpawnPositionWithSpacing()
    {
        Vector3 spawnPosition;
        bool validPosition;

        do
        {
            validPosition = true;
            spawnPosition = new Vector3(
                spawnXStart + Random.Range(-spawnXRange, spawnXRange),  // Ahora genera en X
                0f,
                spawnZStart + Random.Range(-spawnZRange+ spawnZRange, spawnZRange)   // Sigue generando en Z
            );

            // Verificar si hay otro cactus demasiado cerca en X o Z
            foreach (GameObject cactus in pool)
            {
                if (cactus.activeInHierarchy && Vector3.Distance(spawnPosition, cactus.transform.position) < 10f) // 8 de largo + 2 de espacio extra
                {
                    validPosition = true; 
                    break;
                }
            }

        } while (!validPosition);

        return spawnPosition;
    }
}