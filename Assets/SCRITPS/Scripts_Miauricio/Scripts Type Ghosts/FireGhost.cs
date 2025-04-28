using UnityEngine;

public class FireGhost : EnemyBase
{
    [Header("Fire Ghost Settings")]
    [SerializeField] private float projectileSpeed = 12f; // Velocidad ajustable desde el Inspector
    [SerializeField] private float predictionMultiplier = 1.3f; // Predicción de movimiento del jugador
    [SerializeField] private float fireballLifetime = 2.5f; // Tiempo antes de autodestrucción
    
    protected override void ShootAtPlayer(GameObject player)
    {
        // 1. Predicción de movimiento del jugador
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        Vector3 predictedPosition = player.transform.position;
        
        if(playerRb != null)
        {
            predictedPosition += playerRb.linearVelocity * predictionMultiplier;
        }

        // 2. Cálculo de dirección mejorado
        Vector3 fireDirection = (predictedPosition - firePoint.position).normalized;

        // Versión corregida:
        GameObject fireball = Instantiate(
        projectilePrefab, 
        firePoint.position, 
        Quaternion.LookRotation(fireDirection)
        );
            
            

        // 4. Configuración física mejorada
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.linearVelocity = fireDirection * projectileSpeed;
            
            // Opcional: Si quieres que las bolas de fuego tengan un efecto arqueado
            // rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }

        // 5. Configuración visual/efectos (opcional)
        if(fireball.GetComponent<TrailRenderer>() != null)
        {
            fireball.GetComponent<TrailRenderer>().Clear();
        }

        // 6. Auto-destrucción controlada
        Destroy(fireball, fireballLifetime);

        Debug.Log("Bola de fuego lanzada con velocidad: " + projectileSpeed);
    }
}