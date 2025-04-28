using UnityEngine;

public class ColdDamageEffect : MonoBehaviour
{
    [Header("UI Visual")]
    public CanvasGroup iceOverlay;
    public float fadeSpeed = 2f;

    [Header("Daño progresivo")]
    public float freezeDuration = 5f;
    public float damageInterval = 1f;
    public float damagePerTick = 5f;

    [Header("Audio")]
    public AudioClip freezeSound;   // 🎵 Sonido de congelación
    public float freezeVolume = 0.5f; // 🔊 Volumen del sonido de congelación
    private AudioSource audioSource;

    private float freezeTimer = 0f;
    private float damageTimer = 0f;
    private bool isFreezing = false;

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
        if (isFreezing)
        {
            freezeTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Mostrar el efecto de hielo
            iceOverlay.alpha = Mathf.Lerp(iceOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            // Reproducir el sonido de congelación si no está sonando
            if (!audioSource.isPlaying && freezeSound != null)
            {
                audioSource.loop = true; // Reproducir en bucle
                audioSource.PlayOneShot(freezeSound, freezeVolume);
            }

            // Aplicar daño progresivo
            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damagePerTick);
                damageTimer = 0f;
            }

            // Fin del efecto
            if (freezeTimer >= freezeDuration)
            {
                freezeTimer = 0f;
                isFreezing = false;
                audioSource.Stop(); // Detener el sonido cuando termine el efecto
            }
        }
        else
        {
            // Ocultar el efecto de hielo suavemente
            iceOverlay.alpha = Mathf.Lerp(iceOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerFreeze()
    {
        isFreezing = true;
        freezeTimer = 0f;
        damageTimer = 0f;
    }
}