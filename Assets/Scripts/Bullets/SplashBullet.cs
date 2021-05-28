using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBullet : GeneralBullet
{
    public float radius;
    bool isItCollision;
    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * _speed);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isItCollision) return;
        isItCollision = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var item in colliders)
        {
            GeneralEnemy generalEnemy = item.GetComponent<GeneralEnemy>();
            if (generalEnemy)
            {
                item.gameObject.GetComponent<GeneralEnemy>().Attack(_damage);
            }              

        }
    }
    //private void ondrawgizmos()
    //{
    //    gizmos.drawsphere(transform.position, radius);
    //}
}
