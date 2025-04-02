using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VRShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private AudioClip revolverSoundClip;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float bulletDamage = 25f;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.AddComponent<ProjectilePlayer>().SetDamage(bulletDamage); // Añadir daño
            bulletPool.Enqueue(bullet);
        }
    }

    public void FireBullet()
    {
        AudioShoot();
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = spawnPoint.rotation;
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();    
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Resetear velocidad primero
                rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);
            }
            StartCoroutine(DeactivateBulletAfterTime(bullet, 3f));
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }

        // Si no hay balas disponibles, instanciar una nueva
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.AddComponent<ProjectilePlayer>().SetDamage(bulletDamage);
        return newBullet;
    }

    private IEnumerator DeactivateBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public void AudioShoot()
    {
        AudioManager.instance.PlaySound(revolverSoundClip);
    }
}

public class ProjectilePlayer: MonoBehaviour
{
    private float damage;

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        Boss enemy = other.GetComponent<Boss>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

