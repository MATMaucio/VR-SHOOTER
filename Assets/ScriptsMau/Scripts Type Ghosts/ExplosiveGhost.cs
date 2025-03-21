using UnityEngine;

public class ExplosiveGhost : EnemyBase
{
    protected override void ShootAtPlayer(GameObject player)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            rb.linearVelocity = direction * 12f;
        }

        Debug.Log("Fantasma explosivo disparó un proyectil explosivo");
    }
}
