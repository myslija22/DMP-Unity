using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject PlayButton;
    private GameObject RaceTrack1;

    public GameObject RaceTrack2;

    public GameObject SelectText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        RaceTrack1.SetActive(true);
        RaceTrack2.SetActive(true);
        SelectText.SetActive(true);
        PlayButton.SetActive(false);

    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
