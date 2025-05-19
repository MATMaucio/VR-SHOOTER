using UnityEngine;

public class ExplosionDamageEffect : MonoBehaviour
{
    [Header("UI Visual")]
    public CanvasGroup explosionOverlay;
    public float fadeSpeed = 3f;

    [Header("Daño instantáneo")]
    public float explosionDamage = 20f;

    [Header("Audio")]
    public AudioClip explosionSound;
    public float explosionVolume = 0.8f;
    private AudioSource audioSource;

    [Header("Efectos secundarios")]
    public float stunDuration = 1.5f;
    public float knockbackForce = 10f;

    private PlayerHealth playerHealth;
    private Rigidbody playerRigidbody;
    private bool isExploding = false;
    private float explosionTimer = 0f;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;

        if (explosionOverlay != null)
            explosionOverlay.alpha = 0f;
    }

    void Update()
    {
        if (isExploding)
        {
            explosionTimer += Time.deltaTime;
            explosionOverlay.alpha = Mathf.Lerp(explosionOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

            if (explosionTimer >= 0.3f)
                isExploding = false;
        }
        else
        {
            explosionOverlay.alpha = Mathf.Lerp(explosionOverlay.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void TriggerExplosion(Vector3 explosionOrigin)
    {
        isExploding = true;
        explosionTimer = 0f;

        playerHealth?.TakeDamage(explosionDamage);

        if (explosionSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(explosionSound, explosionVolume);

        if (playerRigidbody != null)
        {
            Vector3 direction = (transform.position - explosionOrigin).normalized;
            playerRigidbody.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }

        VRShoot vrShoot = FindFirstObjectByType<VRShoot>();
        if (vrShoot != null)
            vrShoot.ApplyDebuff(VRShoot.DebuffType.Explosive, stunDuration);
    }
}
