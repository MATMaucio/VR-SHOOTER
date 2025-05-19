using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    public bool isStatic = true;
    public float moveSpeed = 3f;
    public float spawnDistance = 10f;
    public float oscillationAmplitude = 1f;
    public float oscillationFrequency = 2f;

    private float oscillationOffset;
    private Vector3 randomOffset;
    private float dynamicSpeed;
    private float targetDistanceOffset;

    void Start()
    {
        oscillationOffset = Random.Range(0f, Mathf.PI * 2f);

        randomOffset = new Vector3(
            Random.Range(-4f, 4f),
            0,
            Random.Range(-4f, 4f)
        );

        // Nueva lógica: velocidad y distancia dinámica por fantasma
        dynamicSpeed = moveSpeed * Random.Range(0.8f, 1.2f);
        targetDistanceOffset = Random.Range(-2f, 2f);
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        LookAt(transform, player.transform);
        if (firePoint != null) LookAt(firePoint, player.transform);

        if (!isStatic)
        {
            Vector3 playerForward = player.transform.forward;
            Vector3 centerTarget = player.transform.position + playerForward * (spawnDistance + targetDistanceOffset);

            // Movimiento circular/flotante con Perlin Noise para aleatoriedad
            float noiseX = Mathf.PerlinNoise(Time.time * 0.5f, transform.position.z) * 2f - 1f;
            float noiseZ = Mathf.PerlinNoise(transform.position.x, Time.time * 0.5f) * 2f - 1f;
            Vector3 noise = new Vector3(noiseX, 0, noiseZ);

            Vector3 targetPosition = centerTarget + randomOffset + noise;

            // Movimiento oscilante en Y
            targetPosition.y += Mathf.Sin(Time.time * oscillationFrequency + oscillationOffset) * oscillationAmplitude;

            // Movimiento interpolado
            transform.position = Vector3.Lerp(transform.position, targetPosition, dynamicSpeed * Time.deltaTime);
        }

        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer(player);
            nextFireTime = Time.time + fireRate;
        }
    }

    void LookAt(Transform source, Transform target)
    {
        source.LookAt(target);
        source.Rotate(0, 180, 0);
    }

    protected virtual void ShootAtPlayer(GameObject player)
    {
        float projectileSpeed = 25f;
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.transform.forward = direction;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        Debug.Log("Disparo realizado directamente hacia el jugador con dirección: " + direction);
    }
}
