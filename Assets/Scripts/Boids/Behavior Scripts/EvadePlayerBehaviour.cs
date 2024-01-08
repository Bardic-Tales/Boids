using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Evade Player")]
public class EvadePlayerBehaviour : FlockBehavior
{
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        var player = FindObjectOfType<Player>();

        if(Vector3.Distance(player.transform.position, agent.transform.position) < flock.predatorDetectionRadius)
        {
            Vector2 directionAwayFromPredator = agent.transform.position - player.transform.position;
            // Get the layer name
            string layerName = LayerMask.LayerToName(agent.gameObject.layer);
            
            if(layerName == "Predator" && Vector3.Distance(player.transform.position, agent.transform.position) < 1.5f)
            {
                Destroy(agent.gameObject);
            }
            return directionAwayFromPredator.normalized / directionAwayFromPredator.magnitude;
        }

        return Vector2.zero;
    }
}
