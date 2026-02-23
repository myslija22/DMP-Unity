using UnityEngine;
using FMODUnity;

public class SettingsButtons : MonoBehaviour
{
    public GameObject settingsUI;

    public GameObject startUI;

    public GameObject volumeSlider;

    public GameObject minimapUI;

    FMOD.Studio.Bus masterBus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsUI.SetActive(false);

        FMOD.GUID guid;
        FMOD.Studio.System system = FMODUnity.RuntimeManager.StudioSystem;
        system.lookupID("bus:/", out guid);
        system.getBusByID(guid, out masterBus);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        masterBus.setVolume(savedVolume);
        volumeSlider.GetComponent<UnityEngine.UI.Slider>().value = savedVolume;
    }


    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        startUI.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        startUI.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        masterBus.setVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);

    }

    public void ToggleMinimap(bool showMinimap)
    {
        minimapUI.SetActive(showMinimap);
        PlayerPrefs.SetInt("ShowMinimap", showMinimap ? 1 : 0);
    }
}
