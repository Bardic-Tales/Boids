using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BoidsSpawner : MonoBehaviour
{
    public Flock flockToSpawn;
    public int numBoids = 10;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            flockToSpawn.AddAgent(numBoids, transform.position);
            Destroy(this.gameObject);
        }
    }
}
