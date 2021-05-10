using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] int _enemiesNeedToBeKilled;

    [SerializeField] private EnemySpawner _enemySpawner;

    private int _enemiesKilled;

    private void Awake()
    {
        _enemiesKilled = 0;

        _enemySpawner.OnEnemyReturnToThePool += TryToCompleteLevel;
    }

    private void Start()
    {
        FindObjectOfType<PlayerBase>().OnDamageWasTaken += TryToFinishTheGame;
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
