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

    private float burnTimer = 0f;
    private float damageTimer = 0f;
    private bool isBurning = false;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isBurning)
        {
            burnTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Mostrar el fuego
            fireOverlay.alpha = Mathf.Lerp(fireOverlay.alpha, 1f, Time.deltaTime * fadeSpeed);

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

