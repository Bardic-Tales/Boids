using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(menuName = "Flock/Behavior/Evade Predator")]
public class EvadePredatorBehaviour : FilteredFlockBehavior
{
    public NotSameFlockFilter notSameFlockFilter;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector2.zero;

        //add all points together and average
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        filteredContext = notSameFlockFilter.Filter(agent, filteredContext);
        Vector2 avoidanceVector = Vector2.zero;
        foreach (Transform item in filteredContext)
        {
            Vector2 directionAwayFromPredator = agent.transform.position - item.position;
            avoidanceVector += directionAwayFromPredator.normalized / directionAwayFromPredator.magnitude;
            if (Vector3.Distance(item.position, agent.transform.position) < 0.5f)
            {
                Destroy(agent.gameObject);
            }
        }

        return avoidanceVector;
    }
}
