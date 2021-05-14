using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBullet : MonoBehaviour
{
    [SerializeField] protected int _damage;
    [SerializeField] protected float _speed;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<GeneralEnemy>().Attack(_damage);
        Destroy(gameObject);
    }
}
