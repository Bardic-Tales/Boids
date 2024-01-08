using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Helpers/ConsumablePlacer")]
public class ConsumablePlacer : ScriptableObject
{
    public Rect rectangleBounds;
    public LayerMask circleLayerMask;


    public Vector2 FindFreePosition(float radius, int maxAttempts)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(rectangleBounds.xMin + radius, rectangleBounds.xMax - radius),
                Random.Range(rectangleBounds.yMin + radius, rectangleBounds.yMax - radius)
            );

            Collider2D overlap = Physics2D.OverlapCircle(randomPosition, radius, circleLayerMask);
            if (!overlap)
            {
                // Found a free position
                return randomPosition;
            }
        }

        // Free position not found
        return Vector2.zero;
    }
}
