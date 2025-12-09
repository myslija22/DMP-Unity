using TMPro;
using UnityEngine;

public class LaptimeManager : MonoBehaviour
{
    public TMP_Text bestLaptimeText;
    public TMP_Text lastLaptimeText;
    public GameObject raceManager;



    void Start()
    {

    }


    void Update()
    {
        CheckpointTimer checkpointTimer = raceManager.GetComponent<CheckpointTimer>();

        if (checkpointTimer.bestLapTime == Mathf.Infinity)
        {
            bestLaptimeText.text = "Best Laptime: N/A";
        }
        else
        {
            bestLaptimeText.text = "Best Laptime: " + checkpointTimer.bestLapTime.ToString("F3") + "s";
        }

        if (checkpointTimer.lastLapTime == Mathf.Infinity)
        {
            lastLaptimeText.text = "Last Laptime: N/A";
        }
        else
        {
            lastLaptimeText.text = "Last Laptime: " + checkpointTimer.lastLapTime.ToString("F3") + "s";
        }
    }
}
