using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TargetDummy : MonoBehaviour
{
    public static event Action<TargetDummy> OnDummyDestroyed;
    
    private Renderer objectRenderer;
    private Color originalColor;
    
    private void Start()
    {
        // Obtener el componente Renderer y guardar el color original
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("UN MEDICO!!!!");
            
            // Cambiar a color rojo
            if (objectRenderer != null)
            {
                objectRenderer.material.color = Color.red;
            }
            
            // Volver al color original después de 0.5 segundos
            Invoke("ResetColor", 0.5f);
            
            OnDummyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
    
    private void ResetColor()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }
}
