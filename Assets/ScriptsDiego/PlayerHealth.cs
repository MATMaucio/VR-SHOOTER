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
    [SerializeField] private AudioClip hitSound;
    

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        
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
        public void AudioShoot()
    {
        AudioManager.instance.PlaySound(hitSound);
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
            AudioShoot();
            Destroy(collision.gameObject); // Destruir el proyectil
        }
    }
}