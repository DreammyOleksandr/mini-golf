using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button startGameButton;
    
    [Header("Scene Settings")]
    public string firstLevelName = "Level1";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    void Awake()
    {
        if (startGameButton == null)
        {
            startGameButton = GetComponent<Button>();
        }
        
        if (startGameButton == null)
        {
            Debug.LogError("MainMenuController: No start game button assigned or found!");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        
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
    }
    
    public void StartGame()
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuController: Starting game, loading scene: {firstLevelName}");
        }
        
        SceneManager.LoadScene(firstLevelName);
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