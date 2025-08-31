using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private List<EnemyCost> enemies = new List<EnemyCost>();
    [SerializeField] private EnemyCost boss;
    private int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    private Transform [] spawnLocations;
    private int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    private int waveLimit;
    private int waveMultiplier;
    public static EnemyManager Instance;

    public List<GameObject> spawnedEnemies = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currWave >= waveLimit && spawnedEnemies.Count == 0)
            RoomManager.Instance.OpenRoomDoors();

        if (currWave > 0 && currWave <= waveLimit) {
            if (spawnTimer <= 0)
            {
                //spawn an enemy
                if (enemiesToSpawn.Count > 0)
                {
                    GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocations[Random.Range(0, spawnLocations.Length)].position, Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    spawnTimer = spawnInterval;
                }
                else
                {
                    waveTimer = 0;
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }

            if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
            {
                currWave++;
                GenerateWave();
            }
        }
    }

    public void SpawnBoss(Transform spawnLocation) {
        Instantiate(boss.enemyPrefab, spawnLocation.position, Quaternion.identity);
    }

    public void StartWaves(int waveLimit, int waveDuration, int waveMultiplier, Transform[] spawnLocations)
    {
        this.waveLimit = waveLimit;
        this.waveMultiplier = waveMultiplier;
        this.waveDuration = waveDuration;
        this.spawnLocations = spawnLocations;
        this.currWave = 1;
        this.GenerateWave();
    }

    public void StopWaves() {
        this.currWave = 0;  // reset current wave value so it does not generate enemies in the fixedUpdate method
        foreach (GameObject spawnedEnemy in spawnedEnemies) 
            Destroy(spawnedEnemy);  // eliminate existing enemies from the scene
    }

    public void GenerateWave()
    {
        waveValue = currWave * waveMultiplier;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }

    public void GenerateEnemies()
    {

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class EnemyCost
{
    public GameObject enemyPrefab;
    public int cost;
}