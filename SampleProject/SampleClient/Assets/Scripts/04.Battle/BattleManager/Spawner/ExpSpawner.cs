using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSpawner : MonoBehaviour
{
    public BattleField field; // Assign the Field object in the Inspector
    //private Queue<Exp> spawnedExps = new Queue<Exp>(); // Queue to track spawned Exps
    //private const int MaxExpCount = 100; // Maximum number of Exps allowed

    // Start is called before the first frame update

    public void SpawnExp(Vector3 spawnLocation)
    {
        //// Check if the spawned Exps count exceeds the maximum limit
        //if (spawnedExps.Count >= MaxExpCount)
        //{
        //    // Push the oldest Exp back to the pool
        //    Exp oldestExp = spawnedExps.Dequeue();
        //    oldestExp.gameObject.SetActive(false);
        //    field.ReturnPooledExp(oldestExp);
        //}

        // Spawn a new Exp
        Exp exp = field.GetPooledExp();
        if (exp != null)
        {
            exp.transform.position = spawnLocation;
            exp.gameObject.SetActive(true);
            //spawnedExps.Enqueue(exp); // Add the new Exp to the queue
        }
    }
}
