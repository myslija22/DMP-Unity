using UnityEngine;
using System.Collections.Generic;

public class DeltaTimeManager : MonoBehaviour
{

    public CheckpointTimer checkpointTimer;
    public float currentDeltaTime = 0f;
    public float lastLapDeltaTime = 0f;
    private int lastCurrentSplitCount = 0;
    private List<float> bestSplits = new List<float>();
    private List<float> currentSplits = new List<float>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (checkpointTimer == null) return;
        float bestLapTime = checkpointTimer.GetBestLapTime();
        float lastLapTime = checkpointTimer.GetLastLapTime();
        float currentLapTime = checkpointTimer.GetCurrentLapTime();
        currentSplits = checkpointTimer.GetCurrentSplits();
        bestSplits = checkpointTimer.GetBestSplits();

        Debug.Log($"Current Split Count: {currentSplits.Count}, Best Split Count: {bestSplits.Count}");


        if (currentSplits.Count > lastCurrentSplitCount)
        {
            currentDeltaTime = currentSplits[currentSplits.Count - 1] - (bestSplits.Count > currentSplits.Count - 1 ? bestSplits[currentSplits.Count - 1] : 0f);
            lastCurrentSplitCount = currentSplits.Count;
            Debug.Log($"New split recorded. Current Delta Time for split {currentSplits.Count - 1}: {currentDeltaTime:F3}s");
        }
        else if (currentSplits.Count == bestSplits.Count && currentSplits.Count > 0)
        {
            lastLapDeltaTime = currentLapTime - bestLapTime;
            Debug.Log($"Lap completed. Last Lap Delta Time: {lastLapDeltaTime:F3}s");
        }

    }
}
