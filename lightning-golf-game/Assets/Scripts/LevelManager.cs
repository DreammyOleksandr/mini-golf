using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public float levelCompleteDelay = 2f;
    public bool autoLoadNextLevel = true;
    public string nextLevelName = "";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private bool isLevelCompleting = false;
    
    void OnEnable()
    {
        HoleTrigger.OnLevelCompleted += HandleLevelCompleted;
    }
    
    void OnDisable()
    {
        HoleTrigger.OnLevelCompleted -= HandleLevelCompleted;
    }
    
    void Awake()
    {
        if (string.IsNullOrEmpty(nextLevelName))
        {
            AutoDetectNextLevel();
        }
    }
    
    private void HandleLevelCompleted()
    {
        if (isLevelCompleting) return;
        
        isLevelCompleting = true;
        
        if (showDebugInfo)
        {
            Debug.Log("Level completed! Starting completion sequence...");
        }
        
        StartCoroutine(LevelCompleteSequence());
    }
    
    private IEnumerator LevelCompleteSequence()
    {
        yield return new WaitForSeconds(levelCompleteDelay);
        
        if (autoLoadNextLevel && !string.IsNullOrEmpty(nextLevelName))
        {
            LoadNextLevel();
        }
        else
        {
            if (showDebugInfo)
            {
                Debug.Log("Level complete! Auto-load disabled or no next level specified.");
            }
        }
    }
    
    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelName))
        {
            if (showDebugInfo)
            {
                Debug.LogWarning("No next level specified!");
            }
            return;
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"Loading next level: {nextLevelName}");
        }
        
        SceneManager.LoadScene(nextLevelName);
    }
    
    public void RestartLevel()
    {
        if (showDebugInfo)
        {
            Debug.Log("Restarting current level");
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadLevel(string levelName)
    {
        if (showDebugInfo)
        {
            Debug.Log($"Loading level: {levelName}");
        }
        
        SceneManager.LoadScene(levelName);
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
                
                if (showDebugInfo)
                {
                    Debug.Log($"Auto-detected next level: {nextLevelName}");
                }
            }
        }
    }
}