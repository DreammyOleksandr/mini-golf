using UnityEngine;
using Unity.Collections;
using System.Linq;

public enum PreventionMode 
{ 
    None, 
    Simple 
}

public class BallController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public LineRenderer trajectoryLine;
    private Camera mainCamera;

    [Header("Ball Settings")]
    public float stopVelocity;
    public float shotPower;
    public float maxPower;
    
    [Header("Trajectory Settings")]
    public int maxTrajectoryPoints = 8;
    public int minTrajectoryPoints = 3;
    public float trajectoryTimeStep = 0.1f;
    public float maxTrajectoryTime = 0.15f;
    
    [Header("Collision Prevention")]
    public PreventionMode bouncePreventionMode = PreventionMode.Simple;

    private bool isAiming;
    private bool isIdle;
    private bool isShooting;
    Vector3? worldPoint;
    void Awake()
    {
        mainCamera = Camera.main;
        rb.maxAngularVelocity = 1000;
        isAiming = false;
        isIdle = true; // Initialize ball as idle so aiming can start
        
        if(trajectoryLine != null) {
            trajectoryLine.enabled = false;
        }
    }

    void Update()
    {
        if(rb.linearVelocity.magnitude < stopVelocity){
          ProcessAim();
          if(Input.GetMouseButtonDown(0)){
            if(isIdle) {
              isAiming = true;
              Debug.Log("Started aiming");
            }
          }
          if(Input.GetMouseButtonUp(0)){
            if(isAiming) {
              isShooting = true;
              Debug.Log("Released mouse - shooting");
            }
          }
        }
    }

    void FixedUpdate()
    {
      if(rb.linearVelocity.magnitude < stopVelocity)
      {
        Stop();
      }

      if(isShooting){
        if(worldPoint.HasValue) {
          Shoot(worldPoint.Value);
        }
        isShooting = false;
      }
    }

    private void ProcessAim()
    {
      if(!isAiming && !isIdle) {
        return;
      }

      worldPoint = CastMouseClickRay();

      if(!worldPoint.HasValue)
      {
        if(trajectoryLine != null) {
          trajectoryLine.enabled = false;
        }
        return;
      }
      
      if(isAiming && trajectoryLine != null) {
        Debug.Log("Showing trajectory");
        ShowTrajectory(worldPoint.Value);
      } else if(isAiming && trajectoryLine == null) {
        Debug.LogWarning("Trajectory line is null! Assign LineRenderer in inspector.");
      }
    }

    private Vector3? CastMouseClickRay()
    {
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

      if(Physics.Raycast(ray, out RaycastHit hit)){
        Debug.Log($"Raycast hit: {hit.point} on {hit.collider.name}");
        return hit.point;
      }
      Debug.Log("Raycast missed - no hit detected");
      return null;
    }

    private void Stop()
    {
      if(!rb.isKinematic) {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
      }

      isIdle = true;
    }

    private void Shoot(Vector3 point)
    {
      isAiming = false;
      HideTrajectory();
      Vector3 horizontalWorldPoint = new Vector3(point.x, transform.position.y, point.z);

      Vector3 direction = (transform.position - horizontalWorldPoint).normalized;

      float strength = Vector3.Distance(transform.position, horizontalWorldPoint);
      rb.AddForce(direction * strength * shotPower);
    }
    
    private void ShowTrajectory(Vector3 targetPoint)
    {
      Vector3 horizontalWorldPoint = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
      Vector3 direction = (transform.position - horizontalWorldPoint).normalized;
      float strength = Vector3.Distance(transform.position, horizontalWorldPoint);
      
      // Match the physics used in Shoot() method - AddForce applies impulse
      Vector3 force = direction * strength * shotPower;
      Vector3 velocity = force / rb.mass;
      
      // Calculate dynamic trajectory length based on shot power
      float powerRatio = Mathf.Clamp01(strength / maxPower);
      int dynamicTrajectoryPoints = Mathf.RoundToInt(Mathf.Lerp(minTrajectoryPoints, maxTrajectoryPoints, powerRatio));
      float dynamicTime = maxTrajectoryTime * Mathf.Sqrt(powerRatio); // Use sqrt for better scaling
      float dynamicTimeStep = dynamicTime / dynamicTrajectoryPoints;
      
      trajectoryLine.enabled = true;
      trajectoryLine.positionCount = dynamicTrajectoryPoints;
      
      // Create gradient for fade effect
      Gradient gradient = new Gradient();
      GradientColorKey[] colorKey = new GradientColorKey[2];
      GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
      
      // Color stays the same (white or whatever color you want)
      colorKey[0].color = Color.white;
      colorKey[0].time = 0.0f;
      colorKey[1].color = Color.white;
      colorKey[1].time = 1.0f;
      
      // Alpha fades from full to transparent
      alphaKey[0].alpha = 1.0f;
      alphaKey[0].time = 0.0f;
      alphaKey[1].alpha = 0.8f;
      alphaKey[1].time = 0.7f;
      alphaKey[2].alpha = 0.0f;
      alphaKey[2].time = 1.0f;
      
      gradient.SetKeys(colorKey, alphaKey);
      trajectoryLine.colorGradient = gradient;
      
      Vector3 startPosition = transform.position;
      
      for(int i = 0; i < dynamicTrajectoryPoints; i++) {
        float time = i * dynamicTimeStep;
        Vector3 point = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;
        trajectoryLine.SetPosition(i, point);
      }
    }
    
    private void HideTrajectory()
    {
      if(trajectoryLine != null) {
        trajectoryLine.enabled = false;
      }
    }
}
