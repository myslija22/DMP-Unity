using UnityEngine;
using FMODUnity;

public class SettingsButtons : MonoBehaviour
{
    public GameObject settingsUI;

    public GameObject startUI;

    public GameObject volumeSlider;

    public GameObject minimapUI;

    public float volume = 1.0f;
    FMOD.Studio.Bus masterBus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsUI.SetActive(false);

        FMOD.GUID guid;
        FMOD.Studio.System system = FMODUnity.RuntimeManager.StudioSystem;
        system.lookupID("bus:/", out guid);
        system.getBusByID(guid, out masterBus);
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

    }

    public void ToggleMinimap(bool showMinimap)
    {
        minimapUI.SetActive(showMinimap);
    }
}
