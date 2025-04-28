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
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private float shootCooldown = 0.5f; // Tiempo entre disparos

    [Header("Trajectory Settings")]
    [SerializeField] private bool showPrediction = true;
    [SerializeField] private Color realTrajectoryColor = Color.red;
    [SerializeField] private Color predictionColor = Color.green;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private int trajectoryPoints = 50;
    [SerializeField] private float predictionTimeStep = 0.1f;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private LineRenderer predictionTrajectory;
    private Dictionary<GameObject, LineRenderer> bulletTrajectories = new Dictionary<GameObject, LineRenderer>();
    private List<GameObject> activeBullets = new List<GameObject>();
    private bool canShoot = true; // Control del cooldown

    private void Start()
    {
        InitializePool();
        InitializePredictionTrajectory();
    }

    private void Update()
    {
        if (showPrediction)
        {
            UpdatePredictionTrajectory();
        }
        
        UpdateActiveBulletTrajectories();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.AddComponent<ProjectilePlayer>().SetDamage(bulletDamage);
            bulletPool.Enqueue(bullet);
        }
    }

    private void InitializePredictionTrajectory()
    {
        GameObject predictionObj = new GameObject("PredictionTrajectory");
        predictionTrajectory = predictionObj.AddComponent<LineRenderer>();
        predictionTrajectory.startColor = predictionColor;
        predictionTrajectory.endColor = predictionColor;
        predictionTrajectory.startWidth = lineWidth;
        predictionTrajectory.endWidth = lineWidth;
        predictionTrajectory.positionCount = 0;
        predictionTrajectory.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void FireBullet()
    {
        if (!canShoot) return; // No disparar si está en cooldown
        
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
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);
            }

            // Crear LineRenderer para esta bala
            GameObject trajectoryObj = new GameObject("BulletTrajectory");
            LineRenderer trajectory = trajectoryObj.AddComponent<LineRenderer>();
            trajectory.startColor = realTrajectoryColor;
            trajectory.endColor = realTrajectoryColor;
            trajectory.startWidth = lineWidth;
            trajectory.endWidth = lineWidth;
            trajectory.positionCount = 1;
            trajectory.SetPosition(0, spawnPoint.position);
            trajectory.material = new Material(Shader.Find("Sprites/Default"));

            bulletTrajectories[bullet] = trajectory;
            activeBullets.Add(bullet);

            StartCoroutine(DeactivateBulletAfterTime(bullet, bulletLifetime));
            StartCoroutine(ShootCooldown()); // Iniciar cooldown
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void UpdateActiveBulletTrajectories()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = activeBullets[i];
            
            if (bullet == null || !bullet.activeInHierarchy)
            {
                CleanupBullet(bullet);
                activeBullets.RemoveAt(i);
                continue;
            }

            if (bulletTrajectories.TryGetValue(bullet, out LineRenderer trajectory))
            {
                int newPositionIndex = trajectory.positionCount;
                trajectory.positionCount = newPositionIndex + 1;
                trajectory.SetPosition(newPositionIndex, bullet.transform.position);
            }
        }
    }

    private void UpdatePredictionTrajectory()
    {
        Vector3 startPosition = spawnPoint.position;
        Vector3 startVelocity = spawnPoint.forward * bulletSpeed;

        predictionTrajectory.positionCount = trajectoryPoints;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * predictionTimeStep;
            Vector3 point = startPosition + startVelocity * time;
            
            // Considerar gravedad si la bala tiene Rigidbody
            point += 0.5f * Physics.gravity * time * time;

            // Detectar colisiones
            if (i > 0)
            {
                Vector3 prevPoint = predictionTrajectory.GetPosition(i-1);
                if (Physics.Linecast(prevPoint, point, out RaycastHit hit))
                {
                    predictionTrajectory.positionCount = i + 1;
                    predictionTrajectory.SetPosition(i, hit.point);
                    break;
                }
            }

            predictionTrajectory.SetPosition(i, point);
        }
    }

    private void CleanupBullet(GameObject bullet)
    {
        if (bulletTrajectories.TryGetValue(bullet, out LineRenderer trajectory))
        {
            Destroy(trajectory.gameObject);
            bulletTrajectories.Remove(bullet);
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
        CleanupBullet(bullet);
        bulletPool.Enqueue(bullet);
        activeBullets.Remove(bullet);
    }

    public void AudioShoot()
    {
        AudioManager.instance.PlaySound(revolverSoundClip);
    }

    private void OnDestroy()
    {
        if (predictionTrajectory != null)
        {
            Destroy(predictionTrajectory.gameObject);
        }

        foreach (var trajectory in bulletTrajectories.Values)
        {
            if (trajectory != null)
            {
                Destroy(trajectory.gameObject);
            }
        }
    }
}

public class ProjectilePlayer : MonoBehaviour
{
    private float damage;

    public void SetDamage(float amount)
    {
        damage = amount;
    }

   
}

