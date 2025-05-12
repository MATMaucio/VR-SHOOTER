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
    public AudioClip freezeSound;
    public float freezeVolume = 0.5f;
    private AudioSource audioSource;

    private float freezeTimer = 0f;
    private float damageTimer = 0f;
    private bool isFreezing = false;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        if (iceOverlay != null) iceOverlay.alpha = 0f;
    }

    void Update()
    {
        if (isFreezing)
        {
            freezeTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            iceOverlay.alpha = Mathf.Lerp(iceOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            if (!audioSource.isPlaying && freezeSound != null)
            {
                audioSource.clip = freezeSound;
                audioSource.volume = freezeVolume;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damagePerTick);
                damageTimer = 0f;
            }

            if (freezeTimer >= freezeDuration)
            {
                freezeTimer = 0f;
                isFreezing = false;
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
        else
        {
            iceOverlay.alpha = Mathf.Lerp(iceOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerFreeze()
    {
        isFreezing = true;
        freezeTimer = 0f;
        damageTimer = 0f;

        VRShoot vrShoot = Object.FindFirstObjectByType<VRShoot>();
        if (vrShoot != null)
        {
            vrShoot.ApplyDebuff(VRShoot.DebuffType.Frozen, freezeDuration);
        }
    }
}
