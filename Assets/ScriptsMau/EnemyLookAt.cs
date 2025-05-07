using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        // Encuentra al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player'");
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }
}
