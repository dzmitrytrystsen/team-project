using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralEnemy : MonoBehaviour, IAttackable
{
    [Header("Enemy settings")]
    [SerializeField] protected int _health;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _speed;

    public delegate void GetDamageAction(int healthLeft, GameObject enemyGameObject);
    public event GetDamageAction OnDamageWasTaken;

    public delegate void ReadyToReturnToThePoolAction(GameObject enemyGameObject);
    public event ReadyToReturnToThePoolAction OnReadyToReturnToThePool;

    protected Transform _playerBase;

    protected virtual void Start()
    {
        _playerBase = FindObjectOfType<PlayerBase>().transform;
    }

    protected virtual void Update()
    {
        MoveToPlayerBase();
        RotateTowardsPlayerBase();
    }

    public void Attack(int damage)
    {
        _health -= damage;
        OnDamageWasTaken?.Invoke(_health, gameObject);
    }

    private void MoveToPlayerBase()
    {
        float step = _speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _playerBase.position) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerBase.position, step);
        }
    }

    private void RotateTowardsPlayerBase()
    {
        transform.LookAt(_playerBase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBase>())
        {
            _playerBase.gameObject.GetComponent<PlayerBase>().Attack(_damage);
            OnReadyToReturnToThePool?.Invoke(gameObject);
        }
    }
}
