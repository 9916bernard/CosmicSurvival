using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrier : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f; // Speed at which the EnemyCarrier moves forward

    // Update is called once per frame
    void Update()
    {
        // Move the EnemyCarrier forward based on its local forward direction
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
