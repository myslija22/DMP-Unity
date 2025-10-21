using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckpointTimer : MonoBehaviour
{
    public List<GameObject> Checkpoints = new List<GameObject>();

    private float lastTimestamp;
    private float lapTimestamp;
    private int lapCount;
    private int lastIndex = -1;
    public List<float> splitTimes = new List<float>();
    public List<float> currentSplits = new List<float>();
    public List<float> bestLapSplits = new List<float>();
    public float bestLapTime = Mathf.Infinity;
    public float lastLapTime = Mathf.Infinity;
    //

    public float currentLapTime = 0f;
    public float currentDeltaTime = 0f;
    public float lastLapDeltaTime = 0f;
    private bool timerRunning = false;

    void Start()
    {
        lapCount = 0;
        lastIndex = -1;
        timerRunning = false;
    }

    void Update()
    {
        if (!timerRunning)
        {
            currentLapTime = 0f;
            return;
        }

        float completed = currentSplits.Sum();
        float openSegment = Time.time - lastTimestamp;
        currentLapTime = completed + openSegment;
    }

    public void OnCheckpointTriggered(int index)
    {
        float now = Time.time;
        int expected = (lastIndex + 1) % Mathf.Max(1, Checkpoints.Count);

        if (!timerRunning)
        {
            if (index != expected)
            {
                Debug.Log($"Ignored checkpoint {index} (expected {expected} to start).");
                return;
            }

            timerRunning = true;
            lastTimestamp = now;
            lapTimestamp = now;
            lastIndex = index;
            Debug.Log($"Timer started at checkpoint {index}");
            return;
        }

        if (index != expected)
        {
            Debug.Log($"Ignored checkpoint {index} (expected {expected}).");
            return;
        }

        float segment = now - lastTimestamp;
        splitTimes.Add(segment);
        currentSplits.Add(segment);
        Debug.Log($"Checkpoint {index} hit. Segment time: {segment:F3}s");

        int recordedIndex = currentSplits.Count - 1;
        if (recordedIndex >= 0)
        {
            if (bestLapSplits.Count > recordedIndex)
                currentDeltaTime = currentSplits[recordedIndex] - bestLapSplits[recordedIndex];
            else
                currentDeltaTime = currentSplits[recordedIndex];
            Debug.Log($"New split recorded. Current Delta Time for split {recordedIndex}: {currentDeltaTime:F3}s");
        }

        lastTimestamp = now;
        lastIndex = index;

        if (index == 0 && currentSplits.Count >= Checkpoints.Count)
        {
            float lapTime = currentSplits.Sum();
            lastLapTime = lapTime;
            Debug.Log($"Lap {lapCount} completed: {lapTime:F3}s");

            if (lapTime < bestLapTime)
            {
                bestLapTime = lapTime;
                bestLapSplits = new List<float>(currentSplits);
                Debug.Log($"New best lap: {bestLapTime:F3}s (splits saved)");
            }

            if (bestLapTime < Mathf.Infinity)
                lastLapDeltaTime = lastLapTime - bestLapTime;
            else
                lastLapDeltaTime = 0f;

            lapCount++;
            currentSplits.Clear();
            lapTimestamp = now;

            lastIndex = 0;
            lastTimestamp = now;
            timerRunning = true;
        }
    }

    public List<float> GetCurrentSplits() => new List<float>(currentSplits);
    public List<float> GetBestSplits() => new List<float>(bestLapSplits);
    public float GetBestLapTime() => bestLapTime;
    public float GetLastLapTime() => lastLapTime;
    public float GetCurrentLapTime() => currentLapTime;
}