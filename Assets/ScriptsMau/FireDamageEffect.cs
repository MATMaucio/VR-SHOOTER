using UnityEngine;

public class FireDamageEffect : MonoBehaviour
{
    [Header("UI Visual")]
    public CanvasGroup fireOverlay;
    public float fadeSpeed = 2f;

    [Header("Daño progresivo")]
    public float burnDuration = 5f;
    public float damageInterval = 1f;
    public float damagePerTick = 10f;

    [Header("Audio")]
    public AudioClip burnSound;   // 🎵 Sonido de quemadura
    public float burnVolume = 0.5f; // 🔊 Volumen del sonido de quemadura
    private AudioSource audioSource;

    private float burnTimer = 0f;
    private float damageTimer = 0f;
    private bool isBurning = false;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        // Asegurarse de que haya un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;  // Hacerlo 3D para efectos más inmersivos
        audioSource.playOnAwake = false; // No reproducir sonido al iniciar
    }

    void Update()
    {
        if (isBurning)
        {
            burnTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Mostrar el fuego
            fireOverlay.alpha = Mathf.Lerp(fireOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            // Reproducir el sonido de quemadura si no está sonando
            if (!audioSource.isPlaying && burnSound != null)
            {
                audioSource.loop = true; // Reproducir en bucle
                audioSource.PlayOneShot(burnSound, burnVolume);
            }

            // Daño progresivo
            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damagePerTick);
                damageTimer = 0f;
            }

            // Fin del efecto
            if (burnTimer >= burnDuration)
            {
                burnTimer = 0f;
                isBurning = false;
                audioSource.Stop(); // Detener el sonido cuando termine la quemadura
            }
        }
        else
        {
            // Ocultar el fuego suavemente
            fireOverlay.alpha = Mathf.Lerp(fireOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerBurn()
    {
        isBurning = true;
        burnTimer = 0f;
        damageTimer = 0f;
    }
}
