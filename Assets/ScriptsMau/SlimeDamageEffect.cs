using UnityEngine;

public class SlimeDamageEffect : MonoBehaviour
{
    [Header("UI Visual")]
    public CanvasGroup slimeOverlay;
    public float fadeSpeed = 2f;

    [Header("Daño progresivo")]
    public float slimeDuration = 6f;
    public float damageInterval = 1f;
    public float damagePerTick = 7f;

    [Header("Audio")]
    public AudioClip slimeSound;
    public float slimeVolume = 0.6f;
    private AudioSource audioSource;

    private float slimeTimer = 0f;
    private float damageTimer = 0f;
    private bool isSlimed = false;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        if (slimeOverlay != null) slimeOverlay.alpha = 0f;
    }

    void Update()
    {
        if (isSlimed)
        {
            slimeTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            slimeOverlay.alpha = Mathf.Lerp(slimeOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            if (!audioSource.isPlaying && slimeSound != null)
            {
                audioSource.clip = slimeSound;
                audioSource.volume = slimeVolume;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damagePerTick);
                damageTimer = 0f;
            }

            if (slimeTimer >= slimeDuration)
            {
                slimeTimer = 0f;
                isSlimed = false;
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
        else
        {
            slimeOverlay.alpha = Mathf.Lerp(slimeOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerSlime()
    {
        isSlimed = true;
        slimeTimer = 0f;
        damageTimer = 0f;

        VRShoot vrShoot = FindFirstObjectByType<VRShoot>();
        if (vrShoot != null)
        {
            vrShoot.ApplyDebuff(VRShoot.DebuffType.Slime, slimeDuration);
        }
    }
}

