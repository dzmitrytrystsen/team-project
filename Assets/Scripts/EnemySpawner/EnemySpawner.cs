using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _enemyStartPosition;
    [SerializeField] private int _amountToPool = 10;
    [SerializeField] private float _timeBetweenSpawn = 2f;

    private bool _isLevelRunning;

    private List<GameObject> _enemiesToPool = new List<GameObject>();

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
            GameObject enemyFromPool = GetEnemyFromPool(_enemiesToPool);
            enemyFromPool.transform.position = _enemyStartPosition.position;
            enemyFromPool.gameObject.SetActive(true);

            yield return new WaitForSeconds(_timeBetweenSpawn);
        }
    }

    private GameObject GetEnemyFromPool(List<GameObject> listToPoolFrom)
    {
        for (int i = 0; i < listToPoolFrom.Count; i++)
        {
            if (!listToPoolFrom[i].activeInHierarchy)
            {
                return listToPoolFrom[i];
            }
        }

        return null;
    }

    private void GeneratePoolEnemies(int amountToPool, GameObject enemyPrefab, List<GameObject> listToAdd)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject gameObject = Instantiate<GameObject>(enemyPrefab, transform.position, Quaternion.identity);
            gameObject.transform.SetParent(transform);

            // Subscribe to Enemy event
            gameObject.GetComponent<GeneralEnemy>().OnReadyToReturnToThePool += ReturnEnemyToThePool;
            gameObject.GetComponent<GeneralEnemy>().OnDamageWasTaken += TryToReturnEnemyToThePool;

            gameObject.SetActive(false);
            listToAdd.Add(gameObject);
        }
    }

    private void TryToReturnEnemyToThePool(int enemyHealthLeft, GameObject enemyGameObject)
    {
        if (enemyHealthLeft <= 0)
            ReturnEnemyToThePool(enemyGameObject);
        else
            return;
    }

    private void ReturnEnemyToThePool(GameObject enemyGameObject)
    {
        enemyGameObject.SetActive(false);
    }
}
