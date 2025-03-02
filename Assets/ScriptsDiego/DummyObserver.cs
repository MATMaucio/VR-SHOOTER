using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class DummyObserver : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText; // Referencia al UI TextMeshPro en la escena

    private void Start()
    {
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
        score += 10; // Aumenta el puntaje en 10 por cada Dummy destruido
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}