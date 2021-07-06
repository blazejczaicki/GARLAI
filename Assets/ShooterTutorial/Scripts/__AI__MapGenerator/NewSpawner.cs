using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class NewSpawner : MonoBehaviour
{
    [SerializeField] private PlayerAI playerAI;
    [SerializeField] private MapData mapData;
    [SerializeField] private Transform tileSpawnTemplate;
    [SerializeField] private Enemy enemyTemplate;
    [SerializeField] private int enemiesRemainingToSpawn;
    private LivingEntity playerEntity;
    private Transform playerT;

    [SerializeField] private Wave currentWave;
    private int currentWaveNumber;
 
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    float timeBetweenCampingChecks = 2;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    private bool isDisabled;

    public event System.Action<int> OnNewWave;

    private void Start()
    {
        playerEntity = playerAI.GetComponent<PlayerShooter>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;
        ResetPlayerPosition();
    }

    void ResetPlayerPosition()
    {
        playerAI.transform.position = mapData.GetMapCenter();
        playerAI.CurrentTarget = playerT.position;
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;
        Vector3 spawnPlace = mapData.GetRandomPlace();
        Transform spawnTile = Instantiate(tileSpawnTemplate, spawnPlace, tileSpawnTemplate.rotation, mapData.transform);
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = tileMat.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(spawnTile.gameObject);
        Enemy spawnedEnemy = Instantiate(enemyTemplate, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        playerAI.Enemies.Add(spawnedEnemy);
        spawnedEnemy.PlayerAI = playerAI;
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    private void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            Debug.Log("Elminated");
        }
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColour;
    }
}

