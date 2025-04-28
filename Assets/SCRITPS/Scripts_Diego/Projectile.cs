using UnityEngine;

public enum ProjectileType
{
    Normal,
    Fire,
    Ice,
    Poison
}

public class Projectile : MonoBehaviour
{
    public float damage = 15f;
    public float lifetime = 5f;
    public ProjectileType type = ProjectileType.Normal;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 🔥 Si es de tipo fuego, activa el efecto de quemadura
            if (type == ProjectileType.Fire)
            {
                FireDamageEffect fireEffect = collision.gameObject.GetComponent<FireDamageEffect>();
                if (fireEffect != null)
                {
                    fireEffect.TriggerBurn();
                }
            }

            // El daño se aplica en PlayerHealth, así que este script solo destruye
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
