using UnityEngine;

public class IceGhost : EnemyBase
{
    protected override void ShootAtPlayer(GameObject player)
    {
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f; // Velocidad más lenta para proyectiles de hielo
        }

        Debug.Log("Fantasma de hielo disparó un proyectil de hielo");
    }
}