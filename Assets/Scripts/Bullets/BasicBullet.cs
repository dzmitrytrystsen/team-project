using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : GeneralBullet
{
    Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * _speed);      
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<GeneralEnemy>().Attack(_damage);
        Destroy(gameObject);
    }
}
