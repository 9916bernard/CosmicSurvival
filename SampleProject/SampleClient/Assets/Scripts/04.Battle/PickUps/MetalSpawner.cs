using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSpawner : MonoBehaviour
{
    public BattleField field; // Assign the Field object in the Inspector
    [HideInInspector] public Queue<Metal> spawnedMetals = new Queue<Metal>(); // Queue to track spawned Metals
    private const int MaxMetalCount = 100; // Maximum number of Metals allowed

    // Start is called before the first frame update


    public void SpawnMetal(Vector3 spawnlocation)
    {
        if (spawnedMetals.Count >= MaxMetalCount)
        {
            Metal oldestMetal = spawnedMetals.Dequeue();
            field.ReturnPooledMetal(oldestMetal);
        }

        Metal metal = field.GetPooledMetal();
        if (metal != null)
        {
            metal.transform.position = spawnlocation;
            metal.gameObject.SetActive(true);
            spawnedMetals.Enqueue(metal);
        }
    }
}
