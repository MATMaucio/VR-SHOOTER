using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public Slider slider;

    void Awake()
    {
        GameManager.onDamnage+= UpdateHealtBar;
    }

    void OnDestroy()
    {
        GameManager.onDamnage -= UpdateHealtBar;

        
    }

    void UpdateHealtBar(int ammount )
    {
        if(slider != null)
        slider.value -= ammount;

        print (ammount + " V A L O R ");


    }
}
