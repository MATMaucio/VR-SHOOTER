using UnityEngine;

public class StealthGhost : EnemyBase
{
    [Header("Teletransporte")]
    [SerializeField][Range(3f, 10f)] private float minTeleportDistance = 5f;
    [SerializeField][Range(0f, 3f)] private float maxHeightOffset = 1f;
    [SerializeField][Range(0f, 2f)] private float maxRandomOffset = 1f;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float teleportCooldown = 1f;

    [Header("Disparo")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 2f;

    private float lastTeleportTime;

    protected override void ShootAtPlayer(GameObject player)
    {
        if (Time.time > lastTeleportTime + teleportCooldown)
        {
            TeleportAndShoot(player);
        }
        else
        {
            base.ShootAtPlayer(player);
        }
    }

    private void TeleportAndShoot(GameObject player)
    {
        transform.position = CalculateSafePosition(player);
        lastTeleportTime = Time.time;

        // Disparo mejorado
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, projectileLifetime);
        Debug.Log("Teletransporte + Disparo realizado");
    }

    private Vector3 CalculateSafePosition(GameObject player)
    {
        Vector3 basePos = player.transform.position + player.transform.forward * minTeleportDistance;
        Vector3 randomOffset = new Vector3(
            Random.Range(-maxRandomOffset, maxRandomOffset),
            0,
            Random.Range(-maxRandomOffset, maxRandomOffset)
        );

        Vector3 targetPos = basePos + randomOffset;
        targetPos.y = player.transform.position.y + Random.Range(0, maxHeightOffset);

        return Physics.CheckSphere(targetPos, 0.5f, obstacleLayers) ? basePos : targetPos;
    }
}
