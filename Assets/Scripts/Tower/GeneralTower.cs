using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralTower : MonoBehaviour
{
    [Header("Tower settings")]
    [SerializeField] protected int damage;
    [SerializeField] protected int speed;
    [SerializeField] protected int level;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
}
