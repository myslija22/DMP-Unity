using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeedometerController : MonoBehaviour
{
    public Rigidbody carRigidbody;

    [Header("UI")]

    public TMP_Text speedText;

    public Slider rpmSlider;

    public float speed;

    public float Rpm;

    public GameObject Car_Controller;

    // Update is called once per frame
    void Update()
    {
        if (carRigidbody != null)
        {
            speed = carRigidbody.linearVelocity.magnitude * 3.6f;
        }

        if (speedText != null)
        {
            speedText.text = speed.ToString("0") + " KPH";
        }

        if (Car_Controller != null)
        {
            Car_Controller carControllerScript = Car_Controller.GetComponent<Car_Controller>();
            if (carControllerScript != null)
            {
                Rpm = carControllerScript.Rpm;
                if (rpmSlider != null)
                {
                    rpmSlider.value = Rpm;
                }
            }
        }



    }
}
