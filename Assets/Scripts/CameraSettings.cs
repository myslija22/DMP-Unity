using UnityEngine;

public class CameraSettings : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject hoodCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera.SetActive(true);
        hoodCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mainCamera.activeSelf)
            {
                mainCamera.SetActive(false);
                hoodCamera.SetActive(true);
            }
            else
            {
                mainCamera.SetActive(true);
                hoodCamera.SetActive(false);
            }
        }

    }
}
