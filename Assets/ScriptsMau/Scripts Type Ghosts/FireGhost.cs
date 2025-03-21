using UnityEngine;

public class FireGhost : EnemyBase
{
    protected override void ShootAtPlayer(GameObject player)
    {
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 15f; // Velocidad rápida para bolas de fuego
        }

        Debug.Log("Fantasma de fuego disparó una bola de fuego");
    }
}