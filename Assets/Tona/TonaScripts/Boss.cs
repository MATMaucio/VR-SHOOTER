using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour
{
  [Header("Enemy Stats")]
    public float health = 100f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;

    public enum FireMode { Single, Burst, Automatic }

    void Start()
    {
        StartCoroutine(ChangeFireMode());
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    IEnumerator ChangeFireMode()
    {
        while (true)
        {
            FireMode fireMode = (FireMode)Random.Range(0, 3);
            switch (fireMode)
            {
                case FireMode.Single:
                    FireBullet();
                    break;
                case FireMode.Burst:
                    StartCoroutine(BurstFire());
                    break;
                case FireMode.Automatic:
                    StartCoroutine(AutomaticFire());
                    break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void Shoot()
    {
        FireBullet();
    }

    void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator BurstFire()
    {
        for (int i = 0; i < 3; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator AutomaticFire()
    {
        float fireDuration = 1.5f;
        float timer = 0f;
        while (timer < fireDuration)
        {
            FireBullet();
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

public void TakeDamage(float amount)
{
    health -= amount;
    Debug.Log($"Boss health: {health}"); // Verifica en la consola.
    if (health <= 0)
    {
        Die();
    }
}

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Win");
    }
}
