using UnityEngine;

public class ExplosiveGhost : EnemyBase
{
    [Header("Configuración Básica")]
    [SerializeField] private float projectileSpeed = 7f; // Manteniendo tu velocidad original
    [SerializeField] private float arcHeight = 1f; // Altura opcional del arco

    protected override void ShootAtPlayer(GameObject player)
    {
        // 1. Instanciar el proyectil (igual que tu versión original)
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        // 2. Obtener Rigidbody (como en tu código)
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 3. Cálculo de dirección CON arco parabólico opcional
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            
            // Opcional: Añadir altura al disparo (comenta esta línea si quieres disparo recto)
            direction.y += arcHeight;
            direction.Normalize();

            // 4. Aplicar velocidad (igual que tu versión pero con dirección modificada)
            rb.linearVelocity = direction * projectileSpeed;
            
            // Activar gravedad si usas arco
            rb.useGravity = (arcHeight > 0.1f); 
        }

        // 5. Auto-destrucción después de tiempo (nuevo)
        Destroy(projectile, 3f); 

        Debug.Log("Proyectil explosivo lanzado");
    }
}
