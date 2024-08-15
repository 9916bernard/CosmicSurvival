using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    public BattleField field; // Assign the Field object in the Inspector
    private GameObject LaserPrefab; // Reference to the LaserPickUp prefab
    private Transform baseStation; // Reference to the base station
    private LaserPickUp currentLaserPickup;

    private float lastBlackHoleSpawnTime = 0f;
    private float blackHoleCooldown = 60f; // 1 minute cooldown

    private float lastHpSpawnTime = 0f;
    private int hpSpawnCount = 0;
    private float hpCooldown = 60f; // 1 minute cooldown

    public void Init()
    {
        InitializeLaserPickup();
    }

    public void SpawnHp(Vector3 spawnlocation)
    {
        if (Time.time - lastHpSpawnTime > hpCooldown)
        {
            hpSpawnCount = 0; // Reset HP spawn count after 1 minute
            lastHpSpawnTime = Time.time;
        }

        if (hpSpawnCount < 2)
        {
            HealPickUp hp = field.GetPooledHealPickUp();
            if (hp != null)
            {
                hp.transform.position = spawnlocation;
                hp.gameObject.SetActive(true);
                hpSpawnCount++;
            }
        }
    }

    public void SpawnBlackHole(Vector3 spawnlocation)
    {
        if (Time.time - lastBlackHoleSpawnTime >= blackHoleCooldown)
        {
            var blackHolePrefab = Resources.Load<GameObject>("Battle/Pickup/blackHolePickUp");
            GameObject blackHole = Instantiate(blackHolePrefab, spawnlocation, Quaternion.identity, field.transform); // Set parent to BattleField
            lastBlackHoleSpawnTime = Time.time;
        }
    }

    private void InitializeLaserPickup()
    {
        baseStation = field.baseStation.transform;

        if (LaserPrefab == null)
        {
            LaserPrefab = Resources.Load<GameObject>("Battle/Pickup/LaserPickUp");
        }

        if (baseStation != null && LaserPrefab != null)
        {
            SpawnLaserPickup();
        }
    }

    private void SpawnLaserPickup()
    {
        Vector3 randomPosition = baseStation.position + Random.insideUnitSphere * 10;
        randomPosition.z = 0; // Assuming you are working in 2D

        GameObject laserObject = Instantiate(LaserPrefab, randomPosition, Quaternion.identity, field.transform); // Set parent to BattleField
        currentLaserPickup = laserObject.GetComponent<LaserPickUp>();
        currentLaserPickup.onDestroyed += OnLaserPickupDestroyed;
        laserObject.SetActive(true);
    }

    private void OnLaserPickupDestroyed()
    {
        Vector3 spawnPosition = GetRandomPositionOutOfSight(field._Player.transform.position, 10.0f, 20.0f);
        currentLaserPickup.transform.position = spawnPosition;
        currentLaserPickup.ResetPickup();
    }

    public Vector3 GetRandomPositionOutOfSight(Vector3 playerPosition, float minDistance, float maxDistance)
    {
        Vector3 position;
        do
        {
            position = GetRandomPositionWithinRange(playerPosition, minDistance, maxDistance);
        } while (Vector3.Distance(playerPosition, position) < minDistance);

        return position;
    }

    public Vector3 GetRandomPositionWithinRange(Vector3 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minRadius, maxRadius);

        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);

        return new Vector3(x, y, center.z);
    }
}
