using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class VRShoot : MonoBehaviour
{
    public static Vector3 PlayerPosition { get; private set; } // Posición del jugador
    [Header("Shooting Settings")]
    [SerializeField] private AudioClip revolverSoundClip;
    [SerializeField] private AudioClip reloadSoundClip;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float bulletDamage = 25f;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private float shootCooldown = 0.5f; // Tiempo entre disparos

    [Header("UI Settings")]
    [SerializeField] private Canvas reloadCanvas;
    [SerializeField] private Image reloadProgressBar;
    [SerializeField] private TextMeshProUGUI reloadStatusText;
    [SerializeField] private float reloadCompleteDisplayTime = 0.5f;

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
    private bool isReloading = false;
    private float reloadTimer = 0f;

    private void Start()
    {
        InitializePool();
        InitializePredictionTrajectory();
        InitializeUI();
    }

    private void Update()
    {
        VRShoot.PlayerPosition = transform.position; // Actualizar posición del jugador
        if (showPrediction)
        {
            UpdatePredictionTrajectory();
        }

        UpdateActiveBulletTrajectories();
        UpdateReloadUI();
    }

    private void InitializeUI()
    {
        if (reloadCanvas != null)
        {
            reloadCanvas.gameObject.SetActive(false);
            if (reloadProgressBar != null)
            {
                reloadProgressBar.fillAmount = 0f;
            }
            if (reloadStatusText != null)
            {
                reloadStatusText.text = "";
            }
        }
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
            StartCoroutine(StartReloadProcess()); // Iniciar proceso de recarga
        }
    }

    private IEnumerator StartReloadProcess()
    {
        canShoot = false;
        isReloading = true;
        reloadTimer = 0f;

        // Mostrar UI de recarga
        if (reloadCanvas != null)
        {
            reloadCanvas.gameObject.SetActive(true);
        }

        // Reproducir sonido de recarga
        if (reloadSoundClip != null)
        {
            AudioManager.instance.PlaySound(reloadSoundClip);
        }

        // Esperar el tiempo de recarga
        while (reloadTimer < shootCooldown)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }

        // Mostrar "Recarga completa" brevemente
        if (reloadStatusText != null)
        {
            reloadStatusText.text = "READY";
        }

        yield return new WaitForSeconds(reloadCompleteDisplayTime);

        // Ocultar UI
        if (reloadCanvas != null)
        {
            reloadCanvas.gameObject.SetActive(false);
        }

        isReloading = false;
        canShoot = true;
    }

    private void UpdateReloadUI()
    {
        if (!isReloading || reloadCanvas == null) return;

        // Actualizar barra de progreso
        if (reloadProgressBar != null)
        {
            float progress = Mathf.Clamp01(reloadTimer / shootCooldown);
            reloadProgressBar.fillAmount = progress;
        }

        // Actualizar texto de estado (excepto cuando muestra "READY")
        if (reloadStatusText != null && !reloadStatusText.text.Equals("READY"))
        {
            reloadStatusText.text = "RELOADING...";
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
                Vector3 prevPoint = predictionTrajectory.GetPosition(i - 1);
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