using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BattleField field; // Assign the Field object in the Inspector
    public Transform spawnPoint; // Assign the spawn point in the Inspector
    private float spawnRate; // Time between spawns
    public Transform mainCharacter; // Assign the main character's Transform in the Inspector
    private float spawnRadius; // Radius of the circle
    [SerializeField] private BattleManager battleManager = null; // Reference to the BattleManager script
    [HideInInspector] public EnemyUnit enemy = null;

    [HideInInspector] public float bossSpawnTimer = 0.0f; // Timer to track the time elapsed since the last boss spawn
    private UTBEnemySpawn spawnRec = TABLE.enemyspawn;
    private UTBEnemy enemyRec = TABLE.enemy;
    private List<int> spawnWeights;

    public void Init()
    {
        spawnWeights = new List<int> { enemyRec.Find(1001).SpawnChance, enemyRec.Find(1002).SpawnChance, enemyRec.Find(1003).SpawnChance }; // Weights for enemies 1001, 1002, 1003
        spawnRadius = 10.0f;
        this.SetActiveEx(true);
        StartCoroutine(SpawnEnemy());
    }

    public void stopSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemy()
    {
        
        var normalEnemy = spawnRec.Find(1001);
        int spawnCounter = 0; // Counter to keep track of the spawn ratio
        
        

        while (true)
        {
            spawnRate = normalEnemy.SpawnRate - (battleManager.battleTime / normalEnemy.IncreaseRate);
            if (spawnRate < normalEnemy.MaxSpawnRate)
            {
                spawnRate = normalEnemy.MaxSpawnRate;
            }

            Debug.Log(spawnRate);

            bossSpawnTimer += spawnRate; // Increment the boss spawn timer by the spawn rate interval
            //bossSpawnTimer += Time.deltaTime;
            // Check if it's time to spawn the boss
            if (bossSpawnTimer >= enemyRec.Find(3001).SpawnInterval)
            {
               
                var boss = field.GetPooledEnemyBoss(3001);
                if (boss != null)
                {
                    boss.Init(field._Player, battleManager, battleManager.battleTime, 3001);
                    boss.transform.position = GetRandomPositionOnCircle(field._Player.transform.position, 10.0f);
                    boss.transform.rotation = Quaternion.identity;
                    boss.transform.localScale = Vector3.one * 2; // Set the boss scale to three times its original scale
                    boss.gameObject.SetActive(true);
                }
                bossSpawnTimer = 0.0f; // Reset the boss spawn timer
            }
            else
            {
                int enemyID = GetRandomEnemyID(battleManager.battleTime);

                enemy = field.GetPooledEnemy(enemyID);
                if (enemy != null)
                {
                    // Calculate a random point on the circumference of a circle
                    Vector3 randomSpawnPosition = GetRandomPositionOnCircle(battleManager._Field._Player.transform.position, spawnRadius);

                    // Set the enemy's position and rotation
                    enemy.transform.position = randomSpawnPosition;
                    enemy.transform.rotation = spawnPoint.rotation;
                    enemy.transform.localScale = Vector3.one; // Reset the enemy scale to its original scale
                    enemy.gameObject.SetActive(true);

                    // Increment the spawn counter
                    spawnCounter++;
                }
            }
          
            // Wait for the specified spawn rate before spawning the next enemy
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private int GetRandomEnemyID(float battleTime)
    {
        var enemyRec = TABLE.enemy;
        var weightedEnemyIDs = new List<int>();

        if (battleTime >= enemyRec.Find(1003).SpawnTime) // After 5 minutes
        {
            weightedEnemyIDs.AddRange(GetWeightedList(1001, spawnWeights[0])); // 9/12 times
            weightedEnemyIDs.AddRange(GetWeightedList(1002, spawnWeights[1])); // 2/12 times
            weightedEnemyIDs.AddRange(GetWeightedList(1003, spawnWeights[2])); // 1/12 times
        }
        else if (battleTime > enemyRec.Find(1002).SpawnTime)
        {
            weightedEnemyIDs.AddRange(GetWeightedList(1001, spawnWeights[0])); // 5/6 times
            weightedEnemyIDs.AddRange(GetWeightedList(1002, spawnWeights[1])); // 1/6 times
        }
        else
        {
            weightedEnemyIDs.Add(1001); // Only spawn 1001 enemy before 30 seconds
        }

        // Select a random enemy ID from the weighted list
        int randomIndex = Random.Range(0, weightedEnemyIDs.Count);
        return weightedEnemyIDs[randomIndex];
    }

    private List<int> GetWeightedList(int enemyID, int weight)
    {
        var weightedList = new List<int>();
        for (int i = 0; i < weight; i++)
        {
            weightedList.Add(enemyID);
        }
        return weightedList;
    }

    public Vector3 GetRandomPositionOnCircle(Vector3 center, float radius)
    {
        // Generate a random angle between 0 and 360 degrees
        float angle = Random.Range(0f, 360f);
        float radian = angle * Mathf.Deg2Rad;

        // Calculate the x and y coordinates using the random angle
        float x = center.x + radius * Mathf.Cos(radian);
        float y = center.y + radius * Mathf.Sin(radian);

        // Return the calculated position
        return new Vector3(x, y, -6);
    }

    private void DeactivateSpawner()
    {
        // Deactivate the parent object
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }

        // Deactivate the spawner itself
        gameObject.SetActive(false);
    }
}
