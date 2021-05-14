using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] float _timeBetweenWaves;
    [SerializeField] private List<Wave> _waves;

    [SerializeField] private EnemySpawner _enemySpawner;

    private int _enemiesKilled;
    private int _enemiesNeedToBeKilled;

    private void Awake()
    {
        _enemiesKilled = 0;
        _enemiesNeedToBeKilled = CountAllEnemies(_waves);

        _enemySpawner.OnEnemyWasKilled += TryToCompleteLevel;

        if (_waves.Count < 1)
            throw new Exception("No Waves are set in the " + name);
        else
            _enemySpawner.WavesNeedToBeSpawned(_waves, _timeBetweenWaves);
    }

    private void Start()
    {
        FindObjectOfType<PlayerBase>().OnDamageWasTaken += TryToFinishTheGame;
    }

    private int CountAllEnemies(List<Wave> wavesList)
    {
        int enemiesCount = 0;

        foreach (Wave wave in wavesList)
        {
            enemiesCount += wave.AmountOfEnemies;
        }

        return enemiesCount;
    }

    private void TryToCompleteLevel()
    {
        _enemiesKilled++;

        if (_enemiesKilled >= _enemiesNeedToBeKilled)
        {
            _enemySpawner.OnEnemyReturnToThePool -= TryToCompleteLevel;
            Debug.Log("Level is completed!");
        }
        else
            return;
    }

    private void TryToFinishTheGame(int playerBaseHealthLeft)
    {
        if (playerBaseHealthLeft <= 0)
        {
            FindObjectOfType<PlayerBase>().OnDamageWasTaken -= TryToFinishTheGame;
            Debug.Log("Game is over!");
        }
            
    }
}
