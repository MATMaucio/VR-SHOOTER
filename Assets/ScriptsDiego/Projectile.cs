using UnityEngine;

public enum ProjectileType
{
    Normal,
    Fire,
    Ice,
    Poison, // Agregado para el efecto de slime

    Explosive // Agregado para el efecto de explosión
}

public class Projectile : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public float damage = 15f;
    public float lifetime = 15f;
    public ProjectileType type = ProjectileType.Normal;

    [Header("Audio")]
    public AudioClip impactSound;   // 🎵 Sonido de impacto
    public float impactVolume = 0.7f; // 🔉 Volumen del impacto (0 = silencioso, 1 = máximo)
    private AudioSource audioSource;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Buscar o agregar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el audio
        audioSource.spatialBlend = 1f;   // Hacer el sonido 3D
        audioSource.playOnAwake = false; // No reproducir sonido al aparecer
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reproducir sonido de impacto solo si hay un sonido de impacto definido
            if (impactSound != null)
            {
                audioSource.PlayOneShot(impactSound, impactVolume);
            }

            // Activar el efecto de quemadura si es proyectil de fuego
            if (type == ProjectileType.Fire)
            {
                FireDamageEffect fireEffect = collision.gameObject.GetComponent<FireDamageEffect>();
                if (fireEffect != null)
                {
                    fireEffect.TriggerBurn(); // Activar quemadura
                }
            }

            // Activar el efecto de congelación si es proyectil de hielo
            if (type == ProjectileType.Ice)
            {
                ColdDamageEffect coldEffect = collision.gameObject.GetComponent<ColdDamageEffect>();
                if (coldEffect != null)
                {
                    coldEffect.TriggerFreeze(); // Activar congelación
                }
            }

            // Activar el efecto de slime si es proyectil de veneno
            if (type == ProjectileType.Poison)
            {
                SlimeDamageEffect slimeEffect = collision.gameObject.GetComponent<SlimeDamageEffect>();
                if (slimeEffect != null)
                {
                    slimeEffect.TriggerSlime(); // Activar efecto de slime
                }
            }

            Destroy(gameObject, 15.0f); // Esperar un poco para que el sonido se escuche antes de destruir el proyectil
        }

        if (type == ProjectileType.Explosive)
        {
            // Si el proyectil es explosivo, activar el efecto de explosión
            ExplosionDamageEffect explosionEffect = collision.gameObject.GetComponent<ExplosionDamageEffect>();
            if (explosionEffect != null)
            {
                explosionEffect.TriggerExplosion(collision.transform.position); // Activar explosión
            }

            Destroy(gameObject, 30.0f); // Esperar un poco para que el sonido se escuche antes de destruir el proyectil
        }
        else
        {
            // Si no es el jugador, destruir el proyectil inmediatamente sin sonido
            Destroy(gameObject, 15.0f);
        }
    }
}