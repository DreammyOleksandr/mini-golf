public class ShotsCounter : MonoBehaviour
{
    [Header("Settings")]
    public bool resetOnLevelComplete = true;
    public bool showDebugInfo = false;

    public static event Action<int> OnShotCountChanged;
    public static event Action OnCounterReset;

    private int currentShots = 0;
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
        if (showDebugInfo)
        {
            Debug.Log($"Level completed with {currentShots} shots!");
        }

        if (resetOnLevelComplete)
        {
            ResetCounter();
        }
    }

    public void RestartLevel()
    {
        ResetCounter();
    }
}