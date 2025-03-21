using UnityEngine;

public class StealthGhost : EnemyBase
{
    public float teleportDistance = 5f; // Distancia mínima para teletransportarse cerca del jugador

    protected override void ShootAtPlayer(GameObject player)
    {
        Vector3 teleportPosition = player.transform.position + Random.onUnitSphere * teleportDistance;
        teleportPosition.y = firePoint.position.y; // Mantén la altura del fantasma
        transform.position = teleportPosition;

        base.ShootAtPlayer(player);

        Debug.Log("Fantasma sigiloso se teletransportó y disparó");
    }
}
