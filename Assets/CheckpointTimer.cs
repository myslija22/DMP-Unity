using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CheckpointTimer : MonoBehaviour
{
    public List<GameObject> Checkpoints = new List<GameObject>();

    private float lapStartTime;
    private int lapCount;
    private int lastIndex = -1;
    private bool timerRunning = false;

    public List<float> currentCheckpointTimes = new List<float>();

    public List<float> bestCheckpointTimes = new List<float>();
    public float bestLapTime = Mathf.Infinity;
    public float lastLapTime = Mathf.Infinity;

    public float currentLapTime = 0f;
    public float currentDeltaTime = 0f;
    public float lastLapDeltaTime = 0f;

    public Slider positiveDeltaSlider;
    public Slider negativeDeltaSlider;

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

        currentLapTime = Time.time - lapStartTime;

        if (currentDeltaTime >= 0)
        {
            positiveDeltaSlider.gameObject.SetActive(true);
            negativeDeltaSlider.gameObject.SetActive(false);
            positiveDeltaSlider.value = Mathf.Min(currentDeltaTime / 1f, 1f);
        }
        else
        {
            positiveDeltaSlider.gameObject.SetActive(false);
            negativeDeltaSlider.gameObject.SetActive(true);
            negativeDeltaSlider.value = Mathf.Min(-currentDeltaTime / 1f, 1f);
        }


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
            lapStartTime = now;
            lastIndex = index;
            currentCheckpointTimes.Clear();
            Debug.Log($"Lap started at checkpoint {index}");
            return;
        }

        if (index != expected)
        {
            Debug.Log($"Ignored checkpoint {index} (expected {expected}).");
            return;
        }

        float checkpointTime = now - lapStartTime;
        currentCheckpointTimes.Add(checkpointTime);
        Debug.Log($"Checkpoint {index} hit at {checkpointTime:F3}s");

        int recordedIndex = currentCheckpointTimes.Count - 1;
        if (bestCheckpointTimes.Count > recordedIndex)
        {
            currentDeltaTime = currentCheckpointTimes[recordedIndex] - bestCheckpointTimes[recordedIndex];
        }
        else
        {
            currentDeltaTime = currentCheckpointTimes[recordedIndex];
        }
        Debug.Log($"Delta at checkpoint {recordedIndex}: {currentDeltaTime:F3}s");

        lastIndex = index;

        if (index == 0 && currentCheckpointTimes.Count >= Checkpoints.Count)
        {
            lastLapTime = checkpointTime;
            Debug.Log($"Lap {lapCount} completed: {lastLapTime:F3}s");

            if (lastLapTime < bestLapTime)
            {
                bestLapTime = lastLapTime;
                bestCheckpointTimes = new List<float>(currentCheckpointTimes);
                Debug.Log($"New best lap: {bestLapTime:F3}s");
            }

            if (bestLapTime < Mathf.Infinity)
                lastLapDeltaTime = lastLapTime - bestLapTime;
            else
                lastLapDeltaTime = 0f;

            lapCount++;

            lapStartTime = now;
            currentCheckpointTimes.Clear();
            lastIndex = 0;
            timerRunning = true;
            Debug.Log("New lap started immediately.");
        }
    }

    public List<float> GetCurrentCheckpointTimes() => new List<float>(currentCheckpointTimes);
    public List<float> GetBestCheckpointTimes() => new List<float>(bestCheckpointTimes);
    public float GetBestLapTime() => bestLapTime;
    public float GetLastLapTime() => lastLapTime;
    public float GetCurrentLapTime() => currentLapTime;
}