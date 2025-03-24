using UnityEngine;

public class StealthGhost : EnemyBase
{
    public float teleportDistance = 5f; // Distancia mínima para teletransportarse cerca del jugador

    protected override void ShootAtPlayer(GameObject player)
{
    // Calcula la posición base enfrente del jugador
    Vector3 teleportPosition = player.transform.position + player.transform.forward * teleportDistance;

    // Agrega un pequeño desplazamiento aleatorio en los ejes x y z
    teleportPosition += new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

    // Eleva ligeramente al fantasma para que no aparezca en el piso
    teleportPosition.y = player.transform.position.y + 1f;

    // Teletransporta al fantasma
    transform.position = teleportPosition;

    base.ShootAtPlayer(player);

    Debug.Log("Fantasma sigiloso se teletransportó enfrente del jugador con un ligero desplazamiento y disparó");
}
}
