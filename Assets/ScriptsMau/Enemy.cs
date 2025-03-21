using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparará el proyectil
    public float fireRate = 2f; // Tiempo entre disparos
    private float nextFireTime = 0f; // Tiempo para el próximo disparo

    void Update()
    {
        // Encuentra al jugador (suponiendo que el jugador tiene el tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Time.time >= nextFireTime)
        {
            ShootAtPlayer(player);
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootAtPlayer(GameObject player)
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        // Instancia el proyectil en el punto de disparo
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Agrega fuerza al proyectil para que se mueva hacia el jugador
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f; // Ajusta la velocidad del proyectil
        }

        Debug.Log("Disparando al jugador");
    }
}
