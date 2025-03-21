using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float lifeTime = 5f; // Tiempo de vida del proyectil antes de destruirse

    void Start()
    {
        // Destruye el proyectil automáticamente después de un tiempo
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mueve el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el proyectil colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El proyectil golpeó al jugador");
            // Aquí puedes implementar lógica para reducir la vida del jugador
            Destroy(gameObject); // Destruye el proyectil
        }
    }
}
