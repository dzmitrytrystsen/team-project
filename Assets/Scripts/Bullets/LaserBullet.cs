using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : GeneralBullet
{
    private LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
            if (hit.collider.gameObject.GetComponent<GeneralEnemy>())
            {
                hit.collider.gameObject.GetComponent<GeneralEnemy>().Attack(_damage);
            }
          
        }
        else lr.SetPosition(1, transform.forward * 5000);
    }

}
