using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject RaceTrack1;

    public GameObject RaceTrack2;

    public GameObject SelectText;

    public GameObject Title;

    public GameObject SmallTitle;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        RaceTrack1.SetActive(true);
        RaceTrack2.SetActive(true);
        SelectText.SetActive(true);
        PlayButton.SetActive(false);
        Title.SetActive(false);
        SmallTitle.SetActive(true);


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
