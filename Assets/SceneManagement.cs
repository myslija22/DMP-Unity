using UnityEngine;


public class SceneManagement : MonoBehaviour
{
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void LoadRaceScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RaceTrack1");
    }
}
