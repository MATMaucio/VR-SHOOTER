using UnityEngine;

public class EnemyBase : MonoBehaviour
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

   protected virtual void ShootAtPlayer(GameObject player)
{
    // 1. Configuración básica
    float projectileSpeed = 25f; // Ajusta según tu juego
    float predictionFactor = 1.2f; // Para anticipar movimiento del jugador
    
    // 2. Calcula dirección con predicción básica
    Vector3 playerVelocity = player.GetComponent<Rigidbody>().linearVelocity;
    Vector3 predictedPosition = player.transform.position + (playerVelocity * predictionFactor);
    
    Vector3 direction = (predictedPosition - firePoint.position).normalized;

    // 3. Creación del proyectil
    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    
    // 4. Configuración física
    Rigidbody rb = projectile.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = direction * projectileSpeed;
        
        // Opcional: Si quieres efecto arqueado (como lanzamiento de granada)
        // rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); 
    }

    // 5. Auto-destrucción después de tiempo (opcional)
    Destroy(projectile, 3f); 
    
    Debug.Log("Disparo realizado con dirección: " + direction);
}
}