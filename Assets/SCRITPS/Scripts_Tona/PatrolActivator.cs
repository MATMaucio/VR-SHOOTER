using UnityEngine;

public class PatrolActivator : MonoBehaviour
{
    public Patrol patrolScript; // Asigna el objeto que tiene el script de patrulla

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("si funko");
            patrolScript.StartPatrolling();
           
        }
    }
}