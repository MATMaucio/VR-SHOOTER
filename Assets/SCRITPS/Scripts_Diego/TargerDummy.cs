using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class TargetDummy : MonoBehaviour
{
    public static event Action<TargetDummy> OnDummyDestroyed;

    private Material objectMaterial;
    public Material damageMat;
    public SkinnedMeshRenderer myMesh;
    Coroutine corDamage;

    public int HP = 3;

    [Header("Boss Settings")]
    public bool isBoss = false;          // Marcar en Inspector si es el Boss
    public string bossName = "Boss";    // Nombre para identificar al Boss (opcional)
    public float winSceneDelay = 1f;  // Tiempo antes de cargar "Win" (para efectos)

    private void Start()
    {
        objectMaterial = myMesh.material;

        // Opcional: Auto-detecta si es Boss por el nombre
        if (gameObject.name.Contains(bossName))
        {
            isBoss = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnDummyDestroyed?.Invoke(this);

            if (corDamage != null)
            {
                StopCoroutine(corDamage);
            }
            corDamage = StartCoroutine(CorDamage(.25f));
        }
    }

    IEnumerator CorDamage(float time)
    {
        myMesh.material = damageMat;
        yield return new WaitForSeconds(time);
        HP--;

        if (HP <= 0)
        {
            Die();
        }
        else
        {
            ResetColor();
        }
    }

    void Die()
    {
        if (isBoss)
        {
            // Comportamiento especial para el Boss
            Debug.Log($"¡{bossName} derrotado!");
            StartCoroutine(LoadWinSceneAfterDelay(winSceneDelay));
        }
        else
        {
            // Comportamiento normal para enemigos
            gameObject.SetActive(false);
        }
    }

    IEnumerator LoadWinSceneAfterDelay(float delay)
    {
        // Opcional: Aquí puedes añadir efectos de muerte del Boss
        // (ej: explosión, animación, etc.)

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Win");
    }

    private void ResetColor()
    {
        myMesh.material = objectMaterial;
    }
}