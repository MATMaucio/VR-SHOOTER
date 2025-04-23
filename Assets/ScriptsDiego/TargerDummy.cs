using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TargetDummy : MonoBehaviour
{
    public static event Action<TargetDummy> OnDummyDestroyed;
    
    private Material objectMaterial;
    private Color originalColor;
    
    private void Start()
    {
        // Obtener el material y guardar el color original
        objectMaterial = GetComponent<Renderer>().material;
        if (objectMaterial != null)
        {
            originalColor = objectMaterial.color;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("UN MEDICO!!!!");
            
            // Cambiar a color rojo directamente en el material
            if (objectMaterial != null)
            {
                objectMaterial.color = Color.red;
            }
            
            // Volver al color original después de 0.5 segundos
            Invoke("ResetColor", 0.5f);
            
            OnDummyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
    
    private void ResetColor()
    {
        if (objectMaterial != null)
        {
            objectMaterial.color = originalColor;
        }
    }
}