using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    private int currentWaypointIndex = 0;
    private bool hasStopped = false;

    void Update()
    {
        if (waypoints.Length == 0 || hasStopped) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Si estamos muy cerca del waypoint, cambiamos al siguiente
        if (direction.magnitude < 0.2f)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                // Último waypoint alcanzado
                speed = 0f;
                hasStopped = true;
            }
        }
    }   
}
