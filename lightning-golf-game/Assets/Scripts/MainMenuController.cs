using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button startGameButton;
    public Button selectLevelButton;
    
    [Header("Scene Settings")]
    public string firstLevelName = "Level1";
    public string levelSelectSceneName = "LevelSelect";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    void Awake()
    {
        if (startGameButton == null || selectLevelButton == null)
        {
            Debug.LogError("MainMenuController: Missing button references!");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        selectLevelButton.onClick.AddListener(SelectLevel);
        
        if (showDebugInfo)
        {
            Debug.Log("MainMenuController: Main menu initialized successfully");
        }
    }
    
    void OnDestroy()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveListener(StartGame);
        }
        if (selectLevelButton != null)
        {
            selectLevelButton.onClick.RemoveListener(SelectLevel);
        }
    }
    
    public void StartGame()
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuController: Starting game, loading scene: {firstLevelName}");
        }
        
        SceneManager.LoadScene(firstLevelName);
    }
    
    public void SelectLevel()
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuController: Opening level select: {levelSelectSceneName}");
        }
        
        SceneManager.LoadScene(levelSelectSceneName);
    }
    
    public void LoadScene(string sceneName)
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuController: Loading scene: {sceneName}");
        }
        
        SceneManager.LoadScene(sceneName);
    }
    
    public void QuitGame()
    {
        if (showDebugInfo)
        {
            Debug.Log("MainMenuController: Quitting game");
        }
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}