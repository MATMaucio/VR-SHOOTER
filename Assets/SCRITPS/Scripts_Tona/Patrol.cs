using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Patrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    private int currentWaypointIndex = 0;
    private bool isPatrolling = false;
    private bool hasStopped = false;

    void Update()
    {
        if (waypoints.Length == 0 || !isPatrolling || hasStopped) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction.magnitude < 0.2f)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                hasStopped = true;
                speed = 0f;
                StartCoroutine(EndAfterDelay());
            }
        }
    }

    public void StartPatrolling()
    {
        isPatrolling = true;
    }

    private IEnumerator EndAfterDelay()
    {
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene("GameOver");
    }
}
