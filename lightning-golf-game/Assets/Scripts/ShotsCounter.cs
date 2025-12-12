using UnityEngine;
using System;

public class ShotsCounter : MonoBehaviour
{
    [Header("Settings")]
    public bool resetOnLevelComplete = true;
    public bool showDebugInfo = true;

    public static event Action<int> OnShotCountChanged;
    public static event Action OnCounterReset;

    private int currentShots = 0;
    private int lastCompletedLevelShots = 0;
    private static ShotsCounter instance;

    public static ShotsCounter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShotsCounter>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ShotsCounter");
                    instance = go.AddComponent<ShotsCounter>();
                }
            }
            return instance;
        }
    }

    public int CurrentShots => currentShots;
    public int LastCompletedLevelShots => lastCompletedLevelShots;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        HoleTrigger.OnLevelCompleted += HandleLevelCompleted;
        if (showDebugInfo)
        {
            Debug.Log("ShotsCounter: Subscribed to HoleTrigger.OnLevelCompleted event");
        }
    }

    void OnDisable()
    {
        HoleTrigger.OnLevelCompleted -= HandleLevelCompleted;
    }

    public void IncrementShot()
    {
        currentShots++;
        OnShotCountChanged?.Invoke(currentShots);

        if (showDebugInfo)
        {
            Debug.Log($"Shot taken! Total shots: {currentShots}");
        }
    }

    public void ResetCounter()
    {
        currentShots = 0;
        OnShotCountChanged?.Invoke(currentShots);
        OnCounterReset?.Invoke();

        if (showDebugInfo)
        {
            Debug.Log("Shot counter reset");
        }
    }

    private void HandleLevelCompleted()
    {
        // Store the shot count before resetting
        lastCompletedLevelShots = currentShots;
        
        if (showDebugInfo)
        {
            Debug.Log($"Level completed with {currentShots} shots! Stored in lastCompletedLevelShots: {lastCompletedLevelShots}");
        }

        if (resetOnLevelComplete)
        {
            ResetCounter();
            if (showDebugInfo)
            {
                Debug.Log($"Counter reset. CurrentShots: {currentShots}, LastCompletedLevelShots: {lastCompletedLevelShots}");
            }
        }
    }

    public void RestartLevel()
    {
        ResetCounter();
    }
}