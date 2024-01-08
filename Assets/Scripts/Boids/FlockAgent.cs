using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity.normalized); // Calculate the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime); // Rotate towards the target rotation
        
        //transform.up = velocity;
        
        //float hAxis = velocity.x;
        //float vAxis = velocity.y;
        //float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        //transform.eulerAngles = new Vector3(0, 0, -zAxis);
        
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = agentFlock.color;
        Gizmos.DrawWireSphere(transform.position, agentFlock.neighborRadius);
    }
}
