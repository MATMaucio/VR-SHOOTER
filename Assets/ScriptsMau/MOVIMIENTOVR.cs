using UnityEngine;

public class MOVIMIENTOVR : MonoBehaviour
{
    public float speed = 1f; // Velocidad de los objetos (ajustada para que el jugador pueda verlo)
    public float moveInterval = 3f; // Intervalo de tiempo entre movimientos
    private float nextMoveTime = 0f; // Tiempo para el próximo movimiento

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextMoveTime = Time.time + moveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextMoveTime)
        {
            MoveTowardsPlayer();
            nextMoveTime = Time.time + moveInterval;
        }
    }

    void MoveTowardsPlayer()
    {
        // Encuentra al jugador (suponiendo que el jugador tiene el tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Calcula la dirección hacia el jugador solo en el eje Z
            Vector3 direction = new Vector3(0, 0, (player.transform.position.z - transform.position.z)).normalized;
            // Mueve el objeto hacia el jugador
            transform.position += direction * speed * Time.deltaTime;
            Debug.Log("Moviendo objeto hacia el jugador en el eje Z");
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Player'");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destruye el objeto si colisiona con una pared
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            Debug.Log("Objeto destruido al colisionar con la pared");
        }
        else
        {
            Debug.Log("Colisión con un objeto que no es una pared: " + collision.gameObject.name);
        }
    }
}