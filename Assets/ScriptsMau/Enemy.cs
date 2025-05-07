using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparará el proyectil
    public float fireRate = 2f; // Tiempo entre disparos
    private float nextFireTime = 0f; // Tiempo para el próximo disparo

    public bool isStatic = true; // Define si el enemigo es estático o persigue al jugador
    public float moveSpeed = 3f; // Velocidad de movimiento para los enemigos que persiguen
    public float spawnDistance = 10f; // Distancia frente al jugador donde aparecerá el enemigo
    public float oscillationAmplitude = 1f; // Amplitud del movimiento oscilante
    public float oscillationFrequency = 2f; // Frecuencia del movimiento oscilante

    private float oscillationOffset; // Offset para que cada fantasma tenga un movimiento único

    void Start()
    {
        // Generar un offset aleatorio para el movimiento oscilante
        oscillationOffset = Random.Range(0f, Mathf.PI * 2f);
    }

void Update()
{
    // Encuentra al jugador (suponiendo que el jugador tiene el tag "Player")
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        // Usa el método personalizado LookAt para que el enemigo mire al jugador
        LookAt(transform, player.transform);

        // Usa el método personalizado LookAt para que el firePoint mire al jugador
        if (firePoint != null)
        {
            LookAt(firePoint, player.transform);
        }

        // Si el enemigo no es estático, persigue al jugador con un movimiento más natural
        if (!isStatic)
        {
            // Calcula una posición frente al jugador con un desplazamiento aleatorio
            Vector3 playerForward = player.transform.forward;
            Vector3 randomOffset = new Vector3(
                Random.Range(-5f, 5f), // Desplazamiento aleatorio en X
                0,                    // Mantén la altura constante
                Random.Range(-5f, 5f) // Desplazamiento aleatorio en Z
            );
            Vector3 targetPosition = player.transform.position + playerForward * spawnDistance + randomOffset;

            // Agrega un movimiento oscilante en el eje Y para hacerlo más natural
            targetPosition.y += Mathf.Sin(Time.time * oscillationFrequency + oscillationOffset) * oscillationAmplitude;

            // Mueve al enemigo hacia la posición objetivo con interpolación suave
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // Dispara al jugador si es el momento adecuado
        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer(player);
            nextFireTime = Time.time + fireRate;
        }
    }
}

// Método personalizado LookAt
// Método personalizado LookAt
void LookAt(Transform source, Transform target)
{
    // Apunta al objetivo
    source.LookAt(target);

    // Ajusta la rotación si el modelo está mal orientado (por ejemplo, si el eje Z apunta hacia atrás)
    source.Rotate(0, 180, 0); // Gira 180 grados en el eje Y
}

    protected virtual void ShootAtPlayer(GameObject player)
    {
        // 1. Configuración básica
        float projectileSpeed = 25f; // Ajusta según tu juego

        // 2. Calcula dirección directamente hacia el jugador
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        // 3. Creación del proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 4. Rotación del proyectil hacia el jugador
        projectile.transform.forward = direction;

        // 5. Configuración física
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed; // Cambiado de linearVelocity a velocity
        }

        Debug.Log("Disparo realizado directamente hacia el jugador con dirección: " + direction);
    }
}