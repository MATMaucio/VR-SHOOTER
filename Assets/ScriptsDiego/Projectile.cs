using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 15f;
    public float lifetime = 5f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // El jugador manejará el daño a través de su PlayerHealth
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}