using UnityEngine;
using UnityEngine.UI;

public class ShotsUI : MonoBehaviour
{
    [Header("UI References")]
    public Text shotCountText;
    
    [Header("Settings")]
    public string textPrefix = "Shots: ";
    public bool showDebugInfo = false;
    
    void Awake()
    {
        if (shotCountText == null)
        {
            shotCountText = GetComponent<Text>();
        }
        
        if (shotCountText == null)
        {
            Debug.LogError("ShotsUI: No Text component found! Please assign shotCountText or attach this script to a Text component.");
            enabled = false;
            return;
        }
    }
    
    void Start()
    {
        UpdateUI(ShotsCounter.Instance.CurrentShots);
    }
    
    void OnEnable()
    {
        ShotsCounter.OnShotCountChanged += UpdateUI;
        ShotsCounter.OnCounterReset += HandleCounterReset;
    }
    
    void OnDisable()
    {
        ShotsCounter.OnShotCountChanged -= UpdateUI;
        ShotsCounter.OnCounterReset -= HandleCounterReset;
    }
    
    private void UpdateUI(int shotCount)
    {
        if (shotCountText != null)
        {
            shotCountText.text = textPrefix + shotCount.ToString();
            
            if (showDebugInfo)
            {
                Debug.Log($"UI updated: {shotCountText.text}");
            }
        }
    }
    
    private void HandleCounterReset()
    {
        UpdateUI(0);
        
        if (showDebugInfo)
        {
            Debug.Log("UI reset to 0 shots");
        }
    }
}