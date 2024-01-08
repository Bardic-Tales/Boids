using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Leadership")]
public class LeadershipBehaviour : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, maintain current alignment
        if (context.Count == 0)
            return agent.transform.up;
        
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        float leaderAngle = 90.0f;
        Transform leaderBoid = null;
        foreach (Transform item in filteredContext)
        {
            var angle = Vector3.Angle(agent.transform.position - item.position, agent.transform.forward);
            if (angle < leaderAngle && angle < 90.0f)
            {
                leaderBoid = item.transform;
                leaderAngle = angle;
            }
        }

        if (leaderBoid != null)
            return leaderBoid.transform.position - agent.transform.position;
        return new Vector2(0, 0);
    }
}
