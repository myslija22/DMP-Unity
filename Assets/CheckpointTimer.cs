using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CheckpointTimer : MonoBehaviour
{
    [Header("Race Management")]
    public GameObject startMenuUI;
    public GameObject inGameUI;
    public Car_Controller carController;
    public Rigidbody carRigidbody;

    public string trackId = "DefaultTrack";

    [Header("Checkpoint System")]
    public List<GameObject> Checkpoints = new List<GameObject>();

    private float lapStartTime;
    private int lapCount;
    private int lastIndex = -1;
    private bool timerRunning = false;

    public List<float> currentCheckpointTimes = new List<float>();

    public List<float> bestCheckpointTimes = new List<float>();
    public float bestLapTime = Mathf.Infinity;
    public float lastLapTime = Mathf.Infinity;

    public List<float> lastLapTimes = new List<float>();

    public float currentLapTime = 0f;
    public float currentDeltaTime = 0f;
    public float lastLapDeltaTime = 0f;

    public Slider positiveDeltaSlider;
    public Slider negativeDeltaSlider;

    public TMP_Text deltaText;

    public TMP_Text lapTimesListText;

    public FMODUnity.StudioEventEmitter emitter;

    void Start()
    {
        lapCount = 0;
        lastIndex = -1;
        timerRunning = false;

        LoadBestTimes();
        LoadLastLapTimes();
        UpdateLapTimesUI();

        PrepareRace();
    }

    // -------------------------------------

    void Update()
    {
        if (timerRunning)
        {
            currentLapTime = Time.time - lapStartTime;
        }
        else
        {
            currentLapTime = 0f;
        }

        deltaText.text = currentDeltaTime >= 0 ? $"+{currentDeltaTime:F3}s" : $"{currentDeltaTime:F3}s";

        if (currentDeltaTime >= 0)
        {
            negativeDeltaSlider.value = 0f;
            positiveDeltaSlider.value = Mathf.Min(currentDeltaTime / 1f, 1f);
        }
        else
        {
            positiveDeltaSlider.value = 0f;
            negativeDeltaSlider.value = Mathf.Min(-currentDeltaTime / 1f, 1f);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            PrepareRace();
        }
    }

    // -------------------------------------

    public void PrepareRace()
    {
        Time.timeScale = 0f;

        UpdateLapTimesUI();

        emitter.SetParameter("Main Menu", 1);

        if (startMenuUI != null) startMenuUI.SetActive(true);

        if (inGameUI != null) inGameUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    // -------------------------------------
    public void StartRace()
    {
        Time.timeScale = 1f;

        emitter.SetParameter("Main Menu", 2);

        if (startMenuUI != null) startMenuUI.SetActive(false);

        if (inGameUI != null) inGameUI.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // -------------------------------------

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

            lastLapTimes.Add(lastLapTime);

            if (lastLapTimes.Count > 10)
            {
                lastLapTimes.RemoveAt(0);
            }
            SaveLastLapTimes();
            UpdateLapTimesUI();

            if (lastLapTime < bestLapTime)
            {
                bestLapTime = lastLapTime;
                bestCheckpointTimes = new List<float>(currentCheckpointTimes);

                SaveBestTimes();

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

    // -------------------------------------

    public List<float> GetCurrentCheckpointTimes() => new List<float>(currentCheckpointTimes);
    public List<float> GetBestCheckpointTimes() => new List<float>(bestCheckpointTimes);
    public float GetBestLapTime() => bestLapTime;
    public float GetLastLapTime() => lastLapTime;
    public float GetCurrentLapTime() => currentLapTime;

    public string FormatTime(float time)
    {
        System.TimeSpan interval = System.TimeSpan.FromSeconds(time);
        return string.Format("{0:D2}:{1:D2}.{2:D3}", interval.Minutes, interval.Seconds, interval.Milliseconds);
    }

    private void UpdateLapTimesUI()
    {
        if (lapTimesListText == null) return;

        string textOutput = "Last 10 Laps:\n";

        for (int i = lastLapTimes.Count - 1; i >= 0; i--)
        {
            textOutput += $"{FormatTime(lastLapTimes[i])}\n";
        }

        lapTimesListText.text = textOutput;
    }

    private void SaveLastLapTimes()
    {
        PlayerPrefs.SetInt($"{trackId}_LastLapTimesCount", lastLapTimes.Count);
        for (int i = 0; i < lastLapTimes.Count; i++)
        {
            PlayerPrefs.SetFloat($"{trackId}_LastLapTimes_{i}", lastLapTimes[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadLastLapTimes()
    {
        lastLapTimes.Clear();
        int count = PlayerPrefs.GetInt($"{trackId}_LastLapTimesCount", 0);

        for (int i = 0; i < count; i++)
        {
            float time = PlayerPrefs.GetFloat($"{trackId}_LastLapTimes_{i}", 0f);
            lastLapTimes.Add(time);
        }
    }

    private void SaveBestTimes()
    {
        PlayerPrefs.SetFloat($"{trackId}_BestLapTime", bestLapTime);

        PlayerPrefs.SetInt($"{trackId}_BestCheckpointCount", bestCheckpointTimes.Count);
        for (int i = 0; i < bestCheckpointTimes.Count; i++)
        {
            PlayerPrefs.SetFloat($"{trackId}_BestCheckpoint_{i}", bestCheckpointTimes[i]);
        }

        PlayerPrefs.Save();
        Debug.Log("Saved best times");
    }

    private void LoadBestTimes()
    {
        bestLapTime = PlayerPrefs.GetFloat($"{trackId}_BestLapTime", Mathf.Infinity);

        bestCheckpointTimes.Clear();
        int count = PlayerPrefs.GetInt($"{trackId}_BestCheckpointCount", 0);
        for (int i = 0; i < count; i++)
        {
            float time = PlayerPrefs.GetFloat($"{trackId}_BestCheckpoint_{i}", 0f);
            bestCheckpointTimes.Add(time);
        }
        Debug.Log("Loaded best times");
    }
}