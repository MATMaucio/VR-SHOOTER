using UnityEngine;

public class SlimeDamageEffect : MonoBehaviour
{
    [Header("UI Visual")]
    public CanvasGroup slimeOverlay;
    public float fadeSpeed = 2f;

    [Header("Daño progresivo")]
    public float slimeDuration = 5f;
    public float damageInterval = 1f;
    public float damagePerTick = 3f;

    [Header("Audio")]
    public AudioClip slimeSound;   // 🎵 Sonido de slime
    public float slimeVolume = 0.5f; // 🔊 Volumen del sonido de slime
    private AudioSource audioSource;

    private float slimeTimer = 0f;
    private float damageTimer = 0f;
    private bool isSlimed = false;

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
        if (isSlimed)
        {
            slimeTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Mostrar el efecto de slime
            slimeOverlay.alpha = Mathf.Lerp(slimeOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            // Reproducir el sonido de slime si no está sonando
            if (!audioSource.isPlaying && slimeSound != null)
            {
                audioSource.loop = true; // Reproducir en bucle
                audioSource.PlayOneShot(slimeSound, slimeVolume);
            }

            // Aplicar daño progresivo
            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damagePerTick);
                damageTimer = 0f;
            }

            // Fin del efecto
            if (slimeTimer >= slimeDuration)
            {
                slimeTimer = 0f;
                isSlimed = false;
                audioSource.Stop(); // Detener el sonido cuando termine el efecto
            }
        }
        else
        {
            // Ocultar el efecto de slime suavemente
            slimeOverlay.alpha = Mathf.Lerp(slimeOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerSlime()
    {
        isSlimed = true;
        slimeTimer = 0f;
        damageTimer = 0f;
    }
}
