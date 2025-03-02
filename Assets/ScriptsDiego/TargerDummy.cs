using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TargetDummy : MonoBehaviour
{
    public static event Action<TargetDummy> OnDummyDestroyed;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("UN MEDICO!!!!");
            OnDummyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
