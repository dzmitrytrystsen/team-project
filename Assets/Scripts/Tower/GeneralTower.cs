using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralTower : MonoBehaviour
{
    [Header("Tower settings")]
    [SerializeField] protected int damage;
    [SerializeField] protected int speed;
    [SerializeField] protected int level;
    [SerializeField] protected bool seeEnemy = false;

    private Transform enemyPosition;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        LookAtEnemy();
    }

    public void AtackEnemy()
    {

    }

    public void LookAtEnemy( )
    {
         if (seeEnemy == true)
         {     
        Vector3 direction = enemyPosition.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
         }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("EnemyTrigg");
            seeEnemy = true;          
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("EnemyStayOnTrigg");
            seeEnemy = true;
            enemyPosition = other.GetComponent<Transform>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("EnemyExit");
            seeEnemy = false;
        }
    }
}
