using UnityEngine;

public class NewIceGhost : EnemyBase
{
protected override void ShootAtPlayer(GameObject player)
{
    Vector3 direction = (player.transform.position - firePoint.position).normalized;
    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

    Rigidbody rb = projectile.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = direction * 10f;
    }

    // Asegurarse que el proyectil tiene el componente Projectile
    Projectile projectileComponent = projectile.GetComponent<Projectile>();
    if (projectileComponent == null)
    {
        projectileComponent = projectile.AddComponent<Projectile>();
        projectileComponent.damage = 15f; // Daño del proyectil de hielo
    }

    // Asignar tag al proyectil
    projectile.tag = "EnemyProjectile";
    
    Debug.Log("Fantasma de hielo disparó un proyectil de hielo");
}
}
