using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [Header("UI References")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;
    
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";
    public string[] levelSceneNames = { "Level1", "Level2", "Level3" };
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    void Awake()
    {
        if (level1Button == null || level2Button == null || level3Button == null || backButton == null)
        {
            Debug.LogError("LevelSelectController: Missing button references!");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        level1Button.onClick.AddListener(() => LoadLevel(0));
        level2Button.onClick.AddListener(() => LoadLevel(1));
        level3Button.onClick.AddListener(() => LoadLevel(2));
        backButton.onClick.AddListener(BackToMainMenu);
        
        if (showDebugInfo)
        {
            Debug.Log("LevelSelectController: Level select initialized successfully");
        }
    }
    
    void OnDestroy()
    {
        if (level1Button != null) level1Button.onClick.RemoveListener(() => LoadLevel(0));
        if (level2Button != null) level2Button.onClick.RemoveListener(() => LoadLevel(1));
        if (level3Button != null) level3Button.onClick.RemoveListener(() => LoadLevel(2));
        if (backButton != null) backButton.onClick.RemoveListener(BackToMainMenu);
    }
    
    private void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelSceneNames.Length)
        {
            Debug.LogError($"LevelSelectController: Invalid level index: {levelIndex}");
            return;
        }
        
        string levelName = levelSceneNames[levelIndex];
        
        if (showDebugInfo)
        {
            Debug.Log($"LevelSelectController: Loading level {levelIndex + 1}: {levelName}");
        }
        
        SceneManager.LoadScene(levelName);
    }
    
    public void LoadLevel1()
    {
        LoadLevel(0);
    }
    
    public void LoadLevel2()
    {
        LoadLevel(1);
    }
    
    public void LoadLevel3()
    {
        LoadLevel(2);
    }
    
    public void BackToMainMenu()
    {
        if (showDebugInfo)
        {
            Debug.Log($"LevelSelectController: Returning to main menu: {mainMenuSceneName}");
        }
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void LoadScene(string sceneName)
    {
        if (showDebugInfo)
        {
            Debug.Log($"LevelSelectController: Loading scene: {sceneName}");
        }
        
        SceneManager.LoadScene(sceneName);
    }
}