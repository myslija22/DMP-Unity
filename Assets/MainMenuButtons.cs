using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Scene");
    }
}
