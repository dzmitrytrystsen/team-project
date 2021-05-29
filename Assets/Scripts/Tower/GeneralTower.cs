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
    GeneralEnemy generalEnemy;

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
    private void OnTriggerEnter(Collider other)
    {
        generalEnemy = other.gameObject.GetComponent<GeneralEnemy>();
        generalEnemy.OnDamageWasTaken += TryToSwapTarget;
    }

    private void OnTriggerStay(Collider other)
    {
        _enemyTransform = other.transform;
       
        Debug.Log("EnemyStayOnTrigg");
        seeEnemy = true;
        //Debug.Log(_enemyTransform.gameObject.GetComponent<GeneralEnemy>().Health);
    }

    private void OnTriggerExit(Collider other)
    {
            Debug.Log("EnemyExit");
            seeEnemy = false;
        generalEnemy.OnDamageWasTaken -= TryToSwapTarget;
    }
   void TryToSwapTarget(int healthLeft, GameObject enemy)
    {
        if (healthLeft <= 0)
        {
            seeEnemy = false;
        }
        Debug.Log("Enemy was damaged");
    }
}
