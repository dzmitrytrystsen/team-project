using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveEnemyType
{
    Small,
    Middle,
    Big
}

[System.Serializable]
public struct Wave
{
    public WaveEnemyType WaveEnemyType;
    public int AmountOfEnemies;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GeneralEnemy _enemyPrefab;
    [SerializeField] private List<Enemy> _enemies;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _enemyStartPosition;
    [SerializeField] private int _amountToPool = 50;
    [SerializeField] private float _timeBetweenSpawn = 2f;

    public delegate void ReturnEnemyAction();
    public event ReturnEnemyAction OnEnemyReturnToThePool;

    public delegate void EnemyIsKilledAction();
    public event EnemyIsKilledAction OnEnemyWasKilled;

    private float _timeBetweenWaves;
    private int _currentWave;
    private bool _isCurrentWaveSpawing;
    [SerializeField] private bool _areAnyWavesToSpawnLeft;

    private List<Wave> _wavesNeedToBeSpawned;

    private Dictionary<WaveEnemyType, List<GeneralEnemy>> _enemiesToPool = new Dictionary<WaveEnemyType, List<GeneralEnemy>>();

    [System.Serializable]
    private struct Enemy
    {
        public WaveEnemyType EnemyType;
        public GeneralEnemy EnemyPrefab;
    }

    private void Awake()
    {
        GeneratePoolListsForEnemies(_enemies);
    }

    private void Start()
    {
        _currentWave = 0;
        _areAnyWavesToSpawnLeft = true;

        StartCoroutine(SpawnNextWave(_currentWave));
    }

    public void WavesNeedToBeSpawned(List<Wave> wavesNeedToBeSpawned, float timeBetweenWaves)
    {
        _wavesNeedToBeSpawned = wavesNeedToBeSpawned;
        _timeBetweenWaves = timeBetweenWaves;
    }

    private void GeneratePoolListsForEnemies(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            _enemiesToPool.Add(enemy.EnemyType, new List<GeneralEnemy>());

            for (int i = 0; i < _amountToPool; i++)
            {
                GeneralEnemy enemyObject = Instantiate(enemy.EnemyPrefab, transform.position, Quaternion.identity);
                enemyObject.gameObject.transform.SetParent(transform);

                // Subscribe to the Enemy events
                enemyObject.OnReadyToReturnToThePool += ReturnEnemyToThePool;
                enemyObject.OnDamageWasTaken += TryToReturnEnemyToThePool;

                enemyObject.gameObject.SetActive(false);
                _enemiesToPool[enemy.EnemyType].Add(enemyObject);
            }
        }
    }

    private IEnumerator SpawnNextWave(int waveToSpawnIndex)
    {
        if (_areAnyWavesToSpawnLeft)
        {
            Wave waveNeesToBeSpawned = _wavesNeedToBeSpawned[waveToSpawnIndex];

            for (int i = 0; i < waveNeesToBeSpawned.AmountOfEnemies; i++)
            {
                _isCurrentWaveSpawing = true;
                SpawnEnemy(waveNeesToBeSpawned.WaveEnemyType);
                yield return new WaitForSeconds(_timeBetweenSpawn);
            }

            _currentWave++;
            if (waveToSpawnIndex < _wavesNeedToBeSpawned.Count - 1)
            {
                _isCurrentWaveSpawing = false;
                StartCoroutine(WaitAndSpawnNextWave());
            }
            else
                _areAnyWavesToSpawnLeft = false;
        }
    }

    private IEnumerator WaitAndSpawnNextWave()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);
        
        StartCoroutine(SpawnNextWave(_currentWave));
    }

    private void SpawnEnemy(WaveEnemyType enemyTypeToSpawn)
    {
        GeneralEnemy enemyFromPool = GetEnemyFromPool(enemyTypeToSpawn);
        enemyFromPool.transform.position = _enemyStartPosition.position;
        enemyFromPool.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (List<GeneralEnemy> enemiesList in _enemiesToPool.Values)
        {
            foreach (GeneralEnemy enemy in enemiesList)
            {
                enemy.OnReadyToReturnToThePool -= ReturnEnemyToThePool;
                enemy.OnDamageWasTaken -= TryToReturnEnemyToThePool;
            }
        }
    }

    private GeneralEnemy GetEnemyFromPool(WaveEnemyType enemyTypeToLookFor)
    {
        List<GeneralEnemy> listToPoolFrom = _enemiesToPool[enemyTypeToLookFor];

        for (int i = 0; i < listToPoolFrom.Count; i++)
        {
            if (!listToPoolFrom[i].gameObject.activeInHierarchy)
            {
                return listToPoolFrom[i];
            }
        }

        return null;
    }

    private void TryToReturnEnemyToThePool(int enemyHealthLeft, GameObject enemyGameObject)
    {
        if (enemyHealthLeft <= 0)
        {
            ReturnEnemyToThePool(enemyGameObject);
            OnEnemyWasKilled?.Invoke();
        }

        else
            return;
    }

    private bool IfAnyEnemyIsActive()
    {
        foreach (List<GeneralEnemy> enemiesList in _enemiesToPool.Values)
        {
            foreach (GeneralEnemy enemy in enemiesList)
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ReturnEnemyToThePool(GameObject enemyGameObject)
    {
        enemyGameObject.SetActive(false);
        enemyGameObject.GetComponent<GeneralEnemy>().ResetHealth();
        OnEnemyReturnToThePool?.Invoke();

        if (!_isCurrentWaveSpawing && _areAnyWavesToSpawnLeft
            && !IfAnyEnemyIsActive())
        {
            StartCoroutine(SpawnNextWave(_currentWave));
        }
    }
}
