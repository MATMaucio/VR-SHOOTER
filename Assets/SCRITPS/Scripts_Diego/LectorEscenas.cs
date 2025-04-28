using UnityEngine;
using UnityEngine.SceneManagement;

public class LectorEscenas : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Play()
    {
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
