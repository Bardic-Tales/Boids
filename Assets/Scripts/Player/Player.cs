using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10;
    public float speedDecelerationRate = 2;
    public float speedAccelerationRate = 2;
    public static bool isPointerDown = false;

    private FixedJoystick _joystick;
    private Vector2 _moveDirection;

    private Vector2 _currentVelocity = Vector2.zero;
    private LayerMask _layerToAvoid;
    
    // Start is called before the first frame update
    void Start()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
        _layerToAvoid = (LayerMask.GetMask("Obstacle"));
    }

    // Update is called once per frame
    public void Update() 
    {
        _moveDirection = Vector2.zero;
        _moveDirection += new Vector2(_joystick.Horizontal, _joystick.Vertical);
        
        AvoidObstacles();
        
        if (!isPointerDown)
        {
            _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, speedDecelerationRate * Time.deltaTime);
        }
        else
        {
            // Update current velocity towards the target direction
            _currentVelocity = Vector3.Lerp(_currentVelocity, _moveDirection.normalized * maxSpeed, speedAccelerationRate * Time.deltaTime);
        }
        
        
        
        float hAxis = _currentVelocity.x;
        float vAxis = _currentVelocity.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        
        transform.eulerAngles = new Vector3(0, 0, -zAxis);
        
        
        // Move the object
        transform.position += (Vector3)_currentVelocity * Time.deltaTime;
    }

    private void AvoidObstacles()
    {
        float obstacleCheckRad = 5f;
        Vector3 forward = transform.TransformDirection(Vector3.up);
        
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, 1.0f, _layerToAvoid);

        foreach (var obstacle in obstacles)
        {
            Vector2 directionAwayFromPredator = transform.position - obstacle.transform.position;
            _moveDirection += directionAwayFromPredator.normalized / directionAwayFromPredator.magnitude;
        }

        RaycastHit2D obstacleCheckHit = Physics2D.Raycast(transform.position, forward, obstacleCheckRad, _layerToAvoid);
        
        if (obstacleCheckHit.collider != null)
        {
            //Debug.DrawRay(transform.position, forward * obstacleCheckRad, Color.red, 0.0f, false);
            // Calculate avoidance direction based on obstacle position
            Vector2 obstacleDirection = obstacleCheckHit.transform.position - transform.position;
            float hitAngle = Vector2.SignedAngle(transform.up, obstacleDirection);
            // Turn left or right depending on which side the obstacle is
            float turnAngle = (hitAngle >= 0) ? -90.0f : 90.0f;
            Quaternion rotation = Quaternion.Euler(0, 0, turnAngle);
            
            _moveDirection += (Vector2)(rotation * transform.up) * 10;
        }
    }
}
