using UnityEngine;

public class PatrolActivator : MonoBehaviour
{
    public Patrol patrolScript;         // Asignar en el Inspector
    public SpawnEnemy spawnEnemyScript; // Asignar en el Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("si funko");

            if (patrolScript != null)
                patrolScript.StartPatrolling();

            if (spawnEnemyScript != null)
                spawnEnemyScript.ActivateSpawner();
        }
    }
}
