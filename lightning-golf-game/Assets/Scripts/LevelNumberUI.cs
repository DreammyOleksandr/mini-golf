using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNumberUI : MonoBehaviour
{
    [Header("UI References")]
    public Text levelNumberText;
    
    [Header("Settings")]
    public string textPrefix = "Level ";
    public bool showDebugInfo = false;
    
    private int currentLevelNumber = 1;
    
    void Awake()
    {
        if (levelNumberText == null)
        {
            levelNumberText = GetComponent<Text>();
        }
        
        if (levelNumberText == null)
        {
            Debug.LogError("LevelNumberUI: No Text component found! Please assign levelNumberText or attach this script to a Text component.");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        DetectLevelNumber();
        UpdateUI();
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DetectLevelNumber();
        UpdateUI();
    }
    
    private void DetectLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName.StartsWith("Level"))
        {
            string numberPart = sceneName.Substring(5);
            if (int.TryParse(numberPart, out int levelNumber))
            {
                currentLevelNumber = levelNumber;
                
                if (showDebugInfo)
                {
                    Debug.Log($"LevelNumberUI: Detected level number: {currentLevelNumber}");
                }
            }
            else
            {
                if (showDebugInfo)
                {
                    Debug.LogWarning($"LevelNumberUI: Could not parse level number from scene name: {sceneName}");
                }
            }
        }
    }
    
    private void UpdateUI()
    {
        if (levelNumberText != null)
        {
            levelNumberText.text = textPrefix + currentLevelNumber.ToString();
            
            if (showDebugInfo)
            {
                Debug.Log($"LevelNumberUI: Updated text to: {levelNumberText.text}");
            }
        }
    }
}