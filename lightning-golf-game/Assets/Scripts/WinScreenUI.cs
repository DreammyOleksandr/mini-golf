using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject winScreenPanel;
    public TextMeshProUGUI levelCompleteText;
    public TextMeshProUGUI shotsText;
    public TextMeshProUGUI starRatingText;
    public Button mainMenuButton;
    public Button nextLevelButton;
    
    [Header("Star Rating Settings")]
    public int threeStar = 3;
    public int twoStar = 5;
    public int oneStar = 8;
    
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";
    public bool autoDetectNextLevel = true;
    public string nextLevelName = "";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private string currentSceneName;
    
    void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        
        if (autoDetectNextLevel && string.IsNullOrEmpty(nextLevelName))
        {
            AutoDetectNextLevel();
        }
        
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(false);
        }
    }
    
    void Start()
    {
        SetupButtons();
    }
    
    void OnEnable()
    {
        HoleTrigger.OnLevelCompleted += HandleLevelCompleted;
    }
    
    void OnDisable()
    {
        HoleTrigger.OnLevelCompleted -= HandleLevelCompleted;
    }
    
    void OnDestroy()
    {
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(LoadNextLevel);
        }
    }
    
    private void SetupButtons()
    {
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(LoadNextLevel);
            
            // Hide next level button if this is the last level
            if (string.IsNullOrEmpty(nextLevelName))
            {
                nextLevelButton.gameObject.SetActive(false);
            }
        }
    }
    
    private void HandleLevelCompleted()
    {
        if (showDebugInfo)
        {
            Debug.Log("WinScreenUI: Level completed, showing win screen");
        }
        
        ShowWinScreen();
    }
    
    public void ShowWinScreen()
    {
        if (winScreenPanel == null)
        {
            Debug.LogError("WinScreenUI: Win screen panel is null!");
            return;
        }
        
        // Get actual shot count or use static value for testing
        int shotCount = GetShotCount();
        
        // Update UI elements
        UpdateLevelCompleteText();
        UpdateShotsText(shotCount);
        UpdateStarRating(shotCount);
        
        // Show the win screen
        winScreenPanel.SetActive(true);
        
        if (showDebugInfo)
        {
            Debug.Log($"WinScreenUI: Showing win screen for {currentSceneName} with {shotCount} shots");
        }
    }
    
    private int GetShotCount()
    {
        // Try to get actual shot count from ShotsCounter
        if (ShotsCounter.Instance != null)
        {
            return ShotsCounter.Instance.CurrentShots;
        }
        
        // Fallback to static value for testing
        return 5;
    }
    
    private void UpdateLevelCompleteText()
    {
        if (levelCompleteText != null)
        {
            levelCompleteText.text = $"{currentSceneName} Complete!";
        }
    }
    
    private void UpdateShotsText(int shotCount)
    {
        if (shotsText != null)
        {
            shotsText.text = $"Shots: {shotCount}";
        }
    }
    
    private void UpdateStarRating(int shotCount)
    {
        if (starRatingText == null) return;
        
        string stars = GetStarRating(shotCount);
        starRatingText.text = stars;
    }
    
    private string GetStarRating(int shotCount)
    {
        if (shotCount <= threeStar)
        {
            return "★★★";
        }
        else if (shotCount <= twoStar)
        {
            return "★★☆";
        }
        else if (shotCount <= oneStar)
        {
            return "★☆☆";
        }
        else
        {
            return "☆☆☆";
        }
    }
    
    public void GoToMainMenu()
    {
        if (showDebugInfo)
        {
            Debug.Log($"WinScreenUI: Going to main menu: {mainMenuSceneName}");
        }
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelName))
        {
            if (showDebugInfo)
            {
                Debug.LogWarning("WinScreenUI: No next level specified!");
            }
            return;
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"WinScreenUI: Loading next level: {nextLevelName}");
        }
        
        SceneManager.LoadScene(nextLevelName);
    }
    
    private void AutoDetectNextLevel()
    {
        if (currentSceneName.StartsWith("Level"))
        {
            string numberPart = currentSceneName.Substring(5);
            if (int.TryParse(numberPart, out int currentLevel))
            {
                int nextLevel = currentLevel + 1;
                nextLevelName = "Level" + nextLevel;
                
                // Check if next level exists (basic validation)
                if (nextLevel > 3) // Assuming 3 levels for now
                {
                    nextLevelName = "";
                }
                
                if (showDebugInfo)
                {
                    Debug.Log($"WinScreenUI: Auto-detected next level: {nextLevelName}");
                }
            }
        }
    }
    
    public void HideWinScreen()
    {
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(false);
        }
    }
}