﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopShooter
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private PlayerAI playerAI;

        public bool devMode;
        public Wave[] waves;
        public Enemy enemy;
        public PlayerShooter player;
        LivingEntity playerEntity;
        Transform playerT;

        private Wave currentWave;
        private int currentWaveNumber;
        private int enemiesRemainingToSpawn;
        private int enemiesRemainingAlive;
        private float nextSpawnTime;

        MapGenerator map;

        float timeBetweenCampingChecks = 2;
        float campThresholdDistance = 1.5f;
        float nextCampCheckTime;
        Vector3 campPositionOld;
        bool isCamping;

        bool isDisabled;

        public event System.Action<int> OnNewWave;

        private void Start()
        {
            playerEntity = FindObjectOfType<PlayerShooter>();
            playerT = playerEntity.transform;

            nextCampCheckTime = timeBetweenCampingChecks + Time.time;
            campPositionOld = playerT.position;
            playerEntity.OnDeath += OnPlayerDeath;

            map = FindObjectOfType<MapGenerator>();
            NextWave();
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

                if (devMode)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        StopCoroutine("SpawnEnemy");
                        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                        {
                            GameObject.Destroy(enemy.gameObject);
                        }
                        NextWave();
                    }
                }
            }
        }

        IEnumerator SpawnEnemy()
        {
            float spawnDelay = 1;
            float tileFlashSpeed = 4;

            Transform spawnTile = map.GetRandomOpenTile();
            if (isCamping)
            {
                spawnTile = map.GetTileFromPosition(playerT.position);
            }
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

            Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
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
            if (enemiesRemainingAlive==0)
            {
                NextWave();
            }
        }

        void ResetPlayerPosition()
        {
            playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
        }

        private void NextWave()
        {
            if (currentWaveNumber > 0)
            {
                AudioManager.instance.PlaySound2D("Level Complete");
            }
            currentWaveNumber++;
            if (currentWaveNumber-1<waves.Length)
            {
                currentWave = waves[currentWaveNumber - 1];
                enemiesRemainingToSpawn = currentWave.enemyCount;
                enemiesRemainingAlive = enemiesRemainingToSpawn;

                if (OnNewWave != null)
                {
                    OnNewWave(currentWaveNumber);
                }
                ResetPlayerPosition();
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
}
