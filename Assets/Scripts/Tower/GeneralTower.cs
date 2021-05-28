using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralTower : MonoBehaviour
{
    [Header("Tower settings")]
    [SerializeField] protected float LookSpeed;
    [SerializeField] protected float waitSpawnBullet;
    [SerializeField] protected int level;
    [SerializeField] protected bool seeEnemy = false;

    [SerializeField] protected Transform spawnTransform;
    [SerializeField] protected GameObject bulletType;

    private Transform _enemyTransform;

    protected virtual void Start()
    {
     
    }

    protected virtual void Update()
    {
        LookAtEnemy();
    }
   

    protected void LookAtEnemy( )
    {
         if (seeEnemy == true)
         {     
          Vector3 direction = _enemyTransform.position - transform.position;
          Quaternion rotation = Quaternion.LookRotation(direction);
          transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
         }
    }

    private void OnTriggerStay(Collider other)
    {
        _enemyTransform = other.transform;
        Debug.Log("EnemyStayOnTrigg");
        seeEnemy = true;
    }

    private void OnTriggerExit(Collider other)
    {
            Debug.Log("EnemyExit");
            seeEnemy = false;
    }
}
