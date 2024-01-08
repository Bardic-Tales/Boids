using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public BoidsSpawner spawner;
    public List<FlockAgent> agents = new List<FlockAgent>();
    public CompositeBehavior behavior;
    public ConsumablePlacer placer;

    [Range(1, 500)]
    public int startingCount = 250;
    public Color color;
    [Range(0f, 3f)]
    public float scale = 1f;
    [Range(0, 100)]
    public int minBoidsToSpawner = 1;
    [Range(2, 100)]
    public int maxBoidsToSpawner = 10;
    const float AgentDensity = 0.08f;
    
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(1f, 10f)]
    public float predatorDetectionRadius = 5.0f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;



    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        StartCoroutine(FlockSpawnerCore());

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                (Vector2)transform.position + Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            newAgent.GetComponentInChildren<SpriteRenderer>().color = color;
            newAgent.transform.localScale = new Vector3(scale, scale, scale);
            newAgent.gameObject.layer = gameObject.layer;
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            if (agent == null)
            {
                //agents.Remove(agent);
                continue;
            }
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
        agents.RemoveAll(item => item == null);
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        Collider2D[] predatorColliders = Physics2D.OverlapCircleAll(agent.transform.position, predatorDetectionRadius, LayerMask.GetMask("Predator"));

        contextColliders = contextColliders.Concat(predatorColliders).ToArray();
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
    
    IEnumerator FlockSpawnerCore()
    {
        while (true)
        {
            int randomWaitTime = Random.Range(5, 10);
            yield return new WaitForSeconds(randomWaitTime);
            SpawnSpawner();
        }
    }

    private void SpawnSpawner()
    {
        BoidsSpawner newSpawner = Instantiate(
            spawner,
            placer.FindFreePosition(2.0f, 1000),
            Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
            transform
        );
        newSpawner.name = "Spawner ";
        newSpawner.flockToSpawn = this;
        newSpawner.GetComponentInChildren<SpriteRenderer>().color = color;
        newSpawner.GetComponent<Light2D>().color = color;
        newSpawner.gameObject.layer = gameObject.layer;
        newSpawner.numBoids = Random.Range(minBoidsToSpawner, maxBoidsToSpawner);
    }

    public void AddAgent(int count, Vector2 position)
    {
        int i = 0;
        while (i < count)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                position,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
            );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            newAgent.GetComponentInChildren<SpriteRenderer>().color = color;
            newAgent.transform.localScale = new Vector3(scale, scale, scale);
            newAgent.gameObject.layer = gameObject.layer;
            agents.Add(newAgent);
            i++;
        }
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = color;
        Gizmos.DrawWireSphere(placer.rectangleBounds.max, 3.0f);
        Gizmos.DrawWireSphere(placer.rectangleBounds.min, 3.0f);
    }
}
