using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [Header("UI References")]
    public Button mainMenuButton;
    
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    void Awake()
    {
        if (mainMenuButton == null)
        {
            mainMenuButton = GetComponent<Button>();
        }
        
        if (mainMenuButton == null)
        {
            Debug.LogError("MainMenuButton: No button component found!");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        
        if (showDebugInfo)
        {
            Debug.Log("MainMenuButton: Main menu button initialized successfully");
        }
    }
    
    void OnDestroy()
    {
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }
    }
    
    public void GoToMainMenu()
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuButton: Returning to main menu: {mainMenuSceneName}");
        }
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void LoadScene(string sceneName)
    {
        if (showDebugInfo)
        {
            Debug.Log($"MainMenuButton: Loading scene: {sceneName}");
        }
        
        SceneManager.LoadScene(sceneName);
    }
}