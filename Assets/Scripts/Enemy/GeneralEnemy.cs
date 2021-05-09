using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralEnemy : MonoBehaviour, IAttackable
{
    [Header("Enemy settings")]
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform playerBasePosition;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        MoveToPlayerBase();
        RotateTowardsPlayerBase();
    }

    public void Attack(int damage)
    {
        health -= damage;
    }

    private void MoveToPlayerBase()
    {
        float step = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, playerBasePosition.position) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerBasePosition.position, step);
        }
    }

    private void RotateTowardsPlayerBase()
    {
        transform.LookAt(playerBasePosition);
    }
}
