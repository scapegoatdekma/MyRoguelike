using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName; // Название волны
        public List<EnemyGroup> enemyGroups;
        public int waveQuota; // Общее количество врагов, которое заспавнится в этой волне
        public float spawnInterval; // Интервал, с которым появляются враги
        public int spawnCount; // Сколько врагов уже заспавнилось в этой волне
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // Список всех волн на уровне
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer; // Таймер для интервала спавна врагов
    public int enemiesAlive;
    public int maxEnemiesAllowed; // Максимальное число врагов на карте
    public bool maxEnemiesReached = false; // Флажок индикации максимального количества врагов
    public float waveInterval; // Интервал волн

    public int totalEnemies; // Всего противников
    public int totalEnemiesDied; // Всего поверженных противников


    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // Лист точек спавна врагов

    Transform player;

    void Start()
    {
        player = FindObjectOfType <PlayerStats>().transform;
        CalculateWaveQuota();
    }

    IEnumerator setGameOverState()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.ChangeState(GameManager.GameState.GameOver);
    }
  
    void Update()
    {
        if(totalEnemies == totalEnemiesDied)
        {
           StartCoroutine(setGameOverState());
        }
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) // Проверяем, закончилась ли волна и начинаем следующую волну
        {
            StartCoroutine(BeginNextWave());
        }
         
        spawnTimer += Time.deltaTime;

        // Если кд спавна прошло спавним врага
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        // Ждём время до спавна следующей волны
        yield return new WaitForSeconds(waveInterval);

        // Если есть ещё волны, запускаем следующую волну
        if(currentWaveCount < waves.Count - 1) 
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        totalEnemies += currentWaveQuota;

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        // Проверка, было ли создано минимальное количество врагов в волне
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached )
        {
            //Создаёт каждый тип врагов до тех пор, пока не появятся все
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //Проверка, было ли создано минимальное количество врагов этого типа
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if(enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return; 
                    }

                    // Спавн противника в рандомной точке неподалёку от игрока
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
    public void OnEnemyKilled()
    {
        enemiesAlive--;
        totalEnemiesDied++;
    }
}
