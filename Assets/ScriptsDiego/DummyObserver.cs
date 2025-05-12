using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DummyObserver : MonoBehaviour
{
    private static DummyObserver instance;

    private int score = 0;
    private int highScore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre escenas

        SceneManager.sceneLoaded += OnSceneLoaded; // Detecta cambios de escena
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        TargetDummy.OnDummyDestroyed += HandleDummyDestroyed;
    }

    private void OnDisable()
    {
        TargetDummy.OnDummyDestroyed -= HandleDummyDestroyed;
    }

    private void HandleDummyDestroyed(TargetDummy dummy)
    {
        score += 10;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Play")
        {
            score = 0;
            UpdateScoreUI();
        }
    }
}