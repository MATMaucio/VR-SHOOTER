using UnityEngine;
using System.Collections;

public class BurstGhost : EnemyBase
{
    [Header("Configuración de Ráfaga")]
    [SerializeField][Range(1, 10)] private int projectileCount = 5;
    [SerializeField][Range(10, 60)] private float spreadAngle = 30f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private float timeBetweenShots = 0.1f;
    [SerializeField] private bool useRandomSpread = true;
    [SerializeField] private float projectileLifetime = 3f; // Auto-destrucción

    protected override void ShootAtPlayer(GameObject player)
    {
        StartCoroutine(BurstFireCoroutine(player));
    }

    private IEnumerator BurstFireCoroutine(GameObject player)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            FireSingleProjectile(player, i);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void FireSingleProjectile(GameObject player, int index)
    {
        Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;
        float angle = CalculateSpreadAngle(index);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 finalDirection = rotation * directionToPlayer;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(finalDirection)
        );

        ApplyProjectilePhysics(projectile, finalDirection);
        Destroy(projectile, projectileLifetime);
    }

    private float CalculateSpreadAngle(int index)
    {
        return useRandomSpread 
            ? Random.Range(-spreadAngle / 2, spreadAngle / 2)
            : -spreadAngle / 2 + (spreadAngle / (projectileCount - 1)) * index;
    }

    private void ApplyProjectilePhysics(GameObject projectile, Vector3 direction)
    {
        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = direction.normalized * projectileSpeed;
            rb.useGravity = false; // Ajusta según necesidad
        }
    }
}