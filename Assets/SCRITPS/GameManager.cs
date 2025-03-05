using System;
using UnityEditor.XR.OpenXR.Features;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameManager instance;
    public static event Action<int> onDamnage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        if(instance == null)
        instance = this;
        
    }
    // Update is called once per frame
    void Update()
    {
        geDamage(1);
    }

    public void geDamage(int ammount){
        onDamnage.Invoke(ammount);
    }
}
