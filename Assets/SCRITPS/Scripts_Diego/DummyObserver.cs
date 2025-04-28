using UnityEngine;
using TMPro;
using System;

public class DummyObserver : MonoBehaviour
{
    private int score = 0;
    private int highScore = 0;

    public TextMeshProUGUI scoreText;      // Texto del puntaje actual
    public TextMeshProUGUI highScoreText;  // Texto del high score (opcional, si lo quieres mostrar)

    private void Start()
    {
        // Cargar el high score guardado
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
        
        // Comprobar si se supera el high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // Guarda en disco
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
}