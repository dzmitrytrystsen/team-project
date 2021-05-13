using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GeneralEnemy _enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _enemyStartPosition;
    [SerializeField] private int _amountToPool = 50;
    [SerializeField] private float _timeBetweenSpawn = 2f;

    public delegate void ReturnEnemyAction();
    public event ReturnEnemyAction OnEnemyReturnToThePool;

    public delegate void EnemyIsKilledAction();
    public event EnemyIsKilledAction OnEnemyWasKilled;

    private bool _isLevelRunning;

    private List<GeneralEnemy> _enemiesToPool = new List<GeneralEnemy>();

    private void Awake()
    {
        _isLevelRunning = true;

        GeneratePoolEnemies(_amountToPool, _enemyPrefab, _enemiesToPool);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemyInTime());
    }

    private IEnumerator SpawnEnemyInTime()
    {
        while (_isLevelRunning)
        {
            GeneralEnemy enemyFromPool = GetEnemyFromPool(_enemiesToPool);
            enemyFromPool.transform.position = _enemyStartPosition.position;
            enemyFromPool.gameObject.SetActive(true);
            yield return new WaitForSeconds(_timeBetweenSpawn);
        }
    }

    private void OnDestroy()
    {
        foreach (GeneralEnemy enemy in _enemiesToPool)
        {
            enemy.GetComponent<GeneralEnemy>().OnReadyToReturnToThePool -= ReturnEnemyToThePool;
            enemy.GetComponent<GeneralEnemy>().OnDamageWasTaken -= TryToReturnEnemyToThePool;
        }
    }

    private GeneralEnemy GetEnemyFromPool(List<GeneralEnemy> listToPoolFrom)
    {
        for (int i = 0; i < listToPoolFrom.Count; i++)
        {
            if (!listToPoolFrom[i].gameObject.activeInHierarchy)
            {
                return listToPoolFrom[i];
            }
        }

        return null;
    }

    private void GeneratePoolEnemies(int amountToPool, GeneralEnemy enemyPrefab, List<GeneralEnemy> listToAdd)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GeneralEnemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.gameObject.transform.SetParent(transform);

            // Subscribe to the Enemy events
            enemy.OnReadyToReturnToThePool += ReturnEnemyToThePool;
            enemy.OnDamageWasTaken += TryToReturnEnemyToThePool;

            enemy.gameObject.SetActive(false);
            listToAdd.Add(enemy);
        }
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

    private void ReturnEnemyToThePool(GameObject enemyGameObject)
    {
        enemyGameObject.SetActive(false);
        enemyGameObject.GetComponent<GeneralEnemy>().ResetHealth();
        OnEnemyReturnToThePool?.Invoke();
    }
}
