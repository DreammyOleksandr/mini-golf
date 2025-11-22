using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    [Header("References")]
    private BallController ballController;
    private Rigidbody rb;
    
    [Header("Settings")]
    public float resetDelay = 1f;
    
    private Vector3 lastValidPosition;
    private bool isOnFloor = false;
    private bool wasMoving = false;
    private float stopTimer = 0f;
    
    void Awake()
    {
        ballController = GetComponent<BallController>();
        rb = GetComponent<Rigidbody>();
        
        if(ballController == null)
        {
            Debug.LogError("FloorDetector requires a BallController component on the same GameObject!");
        }
        
        lastValidPosition = transform.position;
    }
    
    void Update()
    {
        bool isCurrentlyMoving = rb.linearVelocity.magnitude > ballController.stopVelocity;
        
        // Store last valid position when ball starts moving
        if(isCurrentlyMoving && !wasMoving)
        {
            lastValidPosition = transform.position;
        }
        
        // Check if ball should be reset (on floor and not moving)
        if(isOnFloor && !isCurrentlyMoving)
        {
            stopTimer += Time.deltaTime;
            if(stopTimer >= resetDelay)
            {
                ResetBallPosition();
                stopTimer = 0f;
            }
        }
        else
        {
            stopTimer = 0f;
        }
        
        wasMoving = isCurrentlyMoving;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = true;
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = true;
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = false;
            stopTimer = 0f;
        }
    }
    
    private void ResetBallPosition()
    {
        transform.position = lastValidPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        isOnFloor = false;
        stopTimer = 0f;
    }
}