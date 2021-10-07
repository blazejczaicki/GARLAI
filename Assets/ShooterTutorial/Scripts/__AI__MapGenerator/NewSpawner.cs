using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class NewSpawner : MonoBehaviour
{
    [SerializeField] private PlayerAI playerAI;
    [SerializeField] private PlayerMLA playerMLA;
    [SerializeField] private MapData mapData;
    [SerializeField] private Transform tileSpawnTemplate;
    [SerializeField] private Enemy enemyTemplate;
    [SerializeField] private int enemiesToSpawn=10;
    private int enemiesRemainingToSpawn;
    private LivingEntity playerEntity;
    private Transform playerT;

    [SerializeField] private Wave currentWave;
    private int currentWaveNumber;
 
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    private bool isDisabled;

    public float enemySpeed { get; set; }
    public float attackDistanceThreshold { get; set; }

	public PlayerAI PlayerAI { get => playerAI; set => playerAI = value; }
	public PlayerMLA PlayerMLA { get => playerMLA; set => playerMLA = value; }

	public event System.Action<int> OnNewWave;

    private void Start()
    {
        enemiesRemainingToSpawn = enemiesToSpawn;

		if (playerAI==null)
		{
            playerEntity = PlayerMLA.PlayerEnt;
        }
		else
		{
            playerEntity = PlayerAI.GetComponent<PlayerEntity>();
        }
        
        playerT = playerEntity.transform;

        playerEntity.OnDeath += OnPlayerDeath;
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        if (playerAI == null)
        {
            PlayerMLA.CharController.enabled = false;
            PlayerMLA.transform.position = mapData.GetMapCenter();
            //PlayerMLA.CurrentTarget = playerT.position;
            PlayerMLA.CharController.enabled = true;
        }
        else
        {
            playerAI.CharController.enabled = false;
            PlayerAI.transform.position = mapData.GetMapCenter();
            PlayerAI.CurrentTarget = playerT.position;
            playerAI.CharController.enabled = true;
        }
    }

    private void Update()
    {
        //if (!isDisabled)
        //{
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        //}
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

        spawnedEnemy.Pathfinder.speed = enemySpeed;
        spawnedEnemy.attackDistanceThreshold = attackDistanceThreshold;

        if (playerAI == null)
        {
            spawnedEnemy.SetTarget(playerMLA.transform);
            //playerMLA.Enemies.Add(spawnedEnemy);
            mapData.Enemies.Add(spawnedEnemy);
            //spawnedEnemy.PlayerAI = PlayerAI;
        }
        else
        {
            spawnedEnemy.SetTarget(playerAI.transform);
            PlayerAI.Enemies.Add(spawnedEnemy);
            mapData.Enemies.Add(spawnedEnemy);
            spawnedEnemy.PlayerAI = PlayerAI;
        }
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
    }

    public void ResetSpawnersWorld()
	{
        this.StopAllCoroutines();
        nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
        enemiesRemainingToSpawn = enemiesToSpawn;
        ResetPlayerPosition();
        if (playerAI == null)
        {
            playerMLA.ResetPlayerWorld();
        }
        else
        {
            playerAI.ResetPlayerWorld();
        }        
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

