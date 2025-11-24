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
    
    [Header("Force Curve Settings")]
    [Range(0.01f, 1.0f)]
    public float minForceDistance = 0.1f;
    [Range(0.1f, 1.0f)] 
    public float forceDeadZone = 0.3f;
    [Range(1.0f, 4.0f)]
    public float forceCurveExponent = 2.0f;
    
    [Header("Trajectory Settings")]
    public int trajectorySteps = 30;
    public float trajectoryStepSize = 0.1f;
    
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
        isIdle = true;
        
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
            }
          }
          if(Input.GetMouseButtonUp(0)){
            if(isAiming) {
              isShooting = true;
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
        ShowTrajectory(worldPoint.Value);
      }
    }

    private Vector3? CastMouseClickRay()
    {
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

      if(Physics.Raycast(ray, out RaycastHit hit)){
        return hit.point;
      }
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

      float distance = Vector3.Distance(transform.position, horizontalWorldPoint);
      float smoothForce = CalculateSmoothForce(distance);
      rb.AddForce(direction * smoothForce * shotPower);
    }

    private void ShowTrajectory(Vector3 targetPoint)
    {
      if(trajectoryLine == null) return;
      
      Vector3 horizontalTargetPoint = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
      Vector3 direction = (transform.position - horizontalTargetPoint).normalized;
      float distance = Vector3.Distance(transform.position, horizontalTargetPoint);
      
      float smoothForce = CalculateSmoothForce(distance);
      Vector3 velocity = direction * smoothForce * shotPower;
      
      Vector3[] trajectoryPoints = CalculateTrajectoryPoints(transform.position, velocity);
      
      trajectoryLine.enabled = true;
      trajectoryLine.positionCount = trajectoryPoints.Length;
      trajectoryLine.SetPositions(trajectoryPoints);
    }

    private void HideTrajectory()
    {
      if(trajectoryLine != null) {
        trajectoryLine.enabled = false;
      }
    }

    private float CalculateSmoothForce(float distance)
    {
      if(distance <= forceDeadZone)
      {
        return minForceDistance;
      }
      
      float effectiveDistance = distance - forceDeadZone;
      float maxEffectiveDistance = maxPower - forceDeadZone;
      float normalizedDistance = Mathf.Clamp01(effectiveDistance / maxEffectiveDistance);
      
      float curvedForce = Mathf.Pow(normalizedDistance, forceCurveExponent);
      float finalForce = Mathf.Lerp(minForceDistance, maxPower, curvedForce);
      
      return finalForce;
    }

    private Vector3[] CalculateTrajectoryPoints(Vector3 startPosition, Vector3 velocity)
    {
      Vector3[] points = new Vector3[trajectorySteps];
      Vector3 currentPos = startPosition;
      Vector3 currentVel = velocity;
      
      points[0] = currentPos;
      
      for(int i = 1; i < trajectorySteps; i++)
      {
        currentPos += currentVel * trajectoryStepSize;
        currentVel += Physics.gravity * trajectoryStepSize;
        points[i] = currentPos;
      }
      
      return points;
    }
}