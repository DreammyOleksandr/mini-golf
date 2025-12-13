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
    public Button mainMenuButton;
    public Button nextLevelButton;
    
    
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";
    public bool autoDetectNextLevel = true;
    public string nextLevelName = "";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    void Awake()
    {
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
        
        // Show the win screen
        winScreenPanel.SetActive(true);
        
        if (showDebugInfo)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"WinScreenUI: Showing win screen for {currentSceneName} with {shotCount} shots");
        }
    }
    
    private int GetShotCount()
    {
        // Try to get actual shot count from ShotsCounter
        if (ShotsCounter.Instance != null)
        {
            int lastCompletedShots = ShotsCounter.Instance.LastCompletedLevelShots;
            int currentShots = ShotsCounter.Instance.CurrentShots;
            
            if (showDebugInfo)
            {
                Debug.Log($"WinScreenUI: LastCompletedLevelShots: {lastCompletedShots}, CurrentShots: {currentShots}");
            }
            
            // If LastCompletedLevelShots is 0, use CurrentShots as it contains the actual shot count
            // This handles the case where WinScreenUI runs before ShotsCounter in the event chain
            if (lastCompletedShots > 0)
            {
                if (showDebugInfo)
                {
                    Debug.Log($"WinScreenUI: Using LastCompletedLevelShots: {lastCompletedShots}");
                }
                return lastCompletedShots;
            }
            else
            {
                if (showDebugInfo)
                {
                    Debug.Log($"WinScreenUI: LastCompletedLevelShots was 0, using CurrentShots as fallback: {currentShots}");
                }
                return currentShots; // Use current shots regardless of whether it's > 0
            }
        }
        
        // Fallback to static value for testing
        if (showDebugInfo)
        {
            Debug.Log("WinScreenUI: ShotsCounter.Instance is null or all values are 0, using fallback value 5");
        }
        return 5;
    }
    
    private void UpdateLevelCompleteText()
    {
        if (levelCompleteText != null)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string formattedLevelName = FormatLevelName(currentSceneName);
            levelCompleteText.text = $"{formattedLevelName} Complete!";
            
            if (showDebugInfo)
            {
                Debug.Log($"WinScreenUI: Scene name: '{currentSceneName}' -> Formatted: '{formattedLevelName}'");
            }
        }
    }
    
    private string FormatLevelName(string sceneName)
    {
        // Convert "Level1" to "Level 1", "Level2" to "Level 2", etc.
        if (sceneName.StartsWith("Level") && sceneName.Length > 5)
        {
            string numberPart = sceneName.Substring(5);
            return $"Level {numberPart}";
        }
        return sceneName;
    }
    
    private void UpdateShotsText(int shotCount)
    {
        if (shotsText != null)
        {
            shotsText.text = $"Shots: {shotCount}";
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
        string currentSceneName = SceneManager.GetActiveScene().name;
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