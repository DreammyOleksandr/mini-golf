using UnityEngine;
using System;

public class HoleTrigger : MonoBehaviour
{
    [Header("Settings")]
    public float requiredTimeInHole = 1f;
    public string ballTag = "Ball";
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    public static event Action OnLevelCompleted;
    
    private bool ballInHole = false;
    private float timeInHole = 0f;
    private Rigidbody ballRigidbody;
    private BallController ballController;
    private bool levelCompleted = false;
    
    void Update()
    {
        if (ballInHole && !levelCompleted && ballRigidbody != null)
        {
            bool ballIsStopped = ballRigidbody.linearVelocity.magnitude < ballController.stopVelocity;
            
            if (ballIsStopped)
            {
                timeInHole += Time.deltaTime;
                
                if (showDebugInfo)
                {
                    Debug.Log($"Ball in hole for: {timeInHole:F2}s / {requiredTimeInHole}s");
                }
                
                if (timeInHole >= requiredTimeInHole)
                {
                    CompletLevel();
                }
            }
            else
            {
                timeInHole = 0f;
                if (showDebugInfo)
                {
                    Debug.Log("Ball is moving, resetting timer");
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ballTag) && !levelCompleted)
        {
            ballInHole = true;
            ballRigidbody = other.GetComponent<Rigidbody>();
            ballController = other.GetComponent<BallController>();
            timeInHole = 0f;
            
            if (showDebugInfo)
            {
                Debug.Log("Ball entered hole trigger");
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ballTag))
        {
            ballInHole = false;
            timeInHole = 0f;
            ballRigidbody = null;
            ballController = null;
            
            if (showDebugInfo)
            {
                Debug.Log("Ball exited hole trigger");
            }
        }
    }
    
    private void CompletLevel()
    {
        if (levelCompleted) return;
        
        levelCompleted = true;
        
        if (showDebugInfo)
        {
            Debug.Log("Level completed!");
        }
        
        OnLevelCompleted?.Invoke();
    }
    
    void OnValidate()
    {
        if (requiredTimeInHole <= 0f)
        {
            requiredTimeInHole = 1f;
        }
    }
}