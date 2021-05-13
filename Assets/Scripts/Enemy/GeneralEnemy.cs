using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class GeneralEnemy : MonoBehaviour, IAttackable
{
    public int Health => _health;
    public int Damage => _damage;
    public float Speed => _speed;

    [Header("Enemy settings")]
    [SerializeField] protected int _health;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _speed;

    public delegate void GetDamageAction(int healthLeft, GameObject enemyGameObject);
    public event GetDamageAction OnDamageWasTaken;

    public delegate void ReadyToReturnToThePoolAction(GameObject enemyGameObject);
    public event ReadyToReturnToThePoolAction OnReadyToReturnToThePool;

    protected Transform _playerBase;
    protected NavMeshAgent _agent;

    private int _default_health;

    protected virtual void Awake()
    {
        _default_health = _health;
    }

    protected virtual void Start()
    {
        _playerBase = FindObjectOfType<PlayerBase>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        MoveToPlayerBase();
        RotateTowardsPlayerBase();
    }

    public void ResetHealth()
    {
        _health = _default_health;
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
            _agent.SetDestination(_playerBase.position);
        }
    }

    private void RotateTowardsPlayerBase()
    {
        transform.LookAt(_playerBase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerBase playerBase))
        {
            playerBase.Attack(_damage);
            OnReadyToReturnToThePool?.Invoke(gameObject);
        }
    }
}
