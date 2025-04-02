using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;
    
    [Header("Efectos")]
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private AudioClip hitSound;
    
    private AudioSource audioSource;

    void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        // Efectos
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
        
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        UpdateHealthUI();
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / maxHealth);
        }
    }

    private void Die()
    {
        Debug.Log("Jugador murió");
        SceneManager.LoadScene("GameOver");

        // Aquí puedes agregar lógica de muerte (reiniciar nivel, pantalla de game over, etc.)
        // Ejemplo: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Para detectar colisiones con proyectiles
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            // Suponiendo que el proyectil tiene un componente Projectile con un valor de daño
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.damage);
            }
            else
            {
                TakeDamage(10f); // Daño por defecto si el proyectil no tiene componente
            }
            
            Destroy(collision.gameObject); // Destruir el proyectil
        }
    }
}