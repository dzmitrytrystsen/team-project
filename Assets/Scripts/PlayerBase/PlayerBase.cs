using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IAttackable
{
    [Header("Player Base Settings")]
    [SerializeField] private int _health;

    public delegate void GetDamageAction(int healthLeft);
    public event GetDamageAction OnDamageWasTaken;

    public void Attack(int damage)
    {
        _health -= damage;
        OnDamageWasTaken?.Invoke(_health);
    }
}
