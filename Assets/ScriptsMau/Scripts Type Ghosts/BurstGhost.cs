using UnityEngine;

public class BurstGhost : EnemyBase
{
    public int projectileCount = 5; // Número de proyectiles en cada ráfaga
    public float spreadAngle = 30f; // Ángulo de dispersión de los proyectiles

    protected override void ShootAtPlayer(GameObject player)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = -spreadAngle / 2 + spreadAngle / (projectileCount - 1) * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = (rotation * firePoint.forward).normalized * 10f;
            }
        }

        Debug.Log("Fantasma de ráfaga disparó múltiples proyectiles");
    }
}