using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Environmental Awareness")]
public class EnvironmentalAwareness : FlockBehavior
{
    public LayerMask layerToAvoid;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        float obstacleCheckRad = 5.0f;
        Vector3 forward = agent.transform.TransformDirection(Vector3.up);
        
        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, forward, obstacleCheckRad, layerToAvoid);

        if (hit.collider != null)
        {
            Debug.DrawRay(agent.transform.position, forward * obstacleCheckRad, Color.red, 0.0f, false);
            // Calculate avoidance direction based on obstacle position
            Vector2 obstacleDirection = hit.transform.position - agent.transform.position;
            float hitAngle = Vector2.SignedAngle(agent.transform.up, obstacleDirection);

            // Turn left or right depending on which side the obstacle is
            float turnAngle = (hitAngle >= 0) ? -90.0f : 90.0f;
            Quaternion rotation = Quaternion.Euler(0, 0, turnAngle);
            
            return rotation * agent.transform.up;
        }
        else
        {
            //Debug.DrawRay(agent.transform.position, forward * obstacleCheckRad, Color.green, 0.0f, false);
        }
            
        return Vector2.zero;
    }
}
