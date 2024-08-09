using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{
    public BattleField field;
    public Transform baseStation;
    public Transform player;
    private float innerSpawnRadius = 10.0f;
    private float outerSpawnRadius = 30.0f;
    public BattleManager battleManager;

    private List<int> asteroidTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

    public void Init(PlayerController player)
    {
        this.player = player.transform;
        SpawnInitialAstroids(20);
    }

    private void SpawnInitialAstroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomPositionWithinRange(baseStation.position, innerSpawnRadius, outerSpawnRadius);
            int astroidId = GetRandomAstroidId();
            Astroid astroid = field.GetPooledAstroid(astroidId);
            if (astroid != null)
            {
                astroid.transform.position = spawnPosition;
                astroid.transform.rotation = Quaternion.identity;
                astroid.gameObject.SetActive(true);
                astroid.Init(battleManager);
            }
        }
    }

    public void RespawnAstroid(Astroid astroid)
    {
        Vector3 spawnPosition = GetRandomPositionOutOfSight(player.position, 10.0f, 30.0f);
        astroid.transform.position = spawnPosition;
        astroid.transform.rotation = Quaternion.identity;
        astroid.gameObject.SetActive(true);
    }

    public Vector3 GetRandomPositionWithinRange(Vector3 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minRadius, maxRadius);

        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);

        return new Vector3(x, y, center.z);
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

    private int GetRandomAstroidId()
    {
        int randomIndex = Random.Range(0, asteroidTypes.Count);
        return asteroidTypes[randomIndex];
    }
}
