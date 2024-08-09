using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSpawner : MonoBehaviour
{
    public BattleField field; // Assign the Field object in the Inspector

    // Start is called before the first frame update


    public void SpawnMetal(Vector3 spawnlocation)
    {
        Metal metal = field.GetPooledMetal();
        if (metal != null)
        {
            metal.transform.position = spawnlocation;
            metal.gameObject.SetActive(true);
        }
    }
}
