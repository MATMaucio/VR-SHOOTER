using UnityEngine;
using System.Collections.Generic;
using System.Collections; 
public class VRShoot : MonoBehaviour
{
    [SerializeField] private AudioClip revolverSoundClip;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int poolSize = 10;

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
            bullet.GetComponent<Rigidbody>().linearVelocity = spawnPoint.forward * bulletSpeed;
            StartCoroutine(DeactivateBulletAfterTime(bullet, 3f));
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }
        return null;
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
