using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : GeneralTower
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        AttackEnemy();
    }

    void AttackEnemy()
    {
        if (seeEnemy == true)
        {
            bulletType.SetActive(true);
        }
        else
            bulletType.SetActive(false);
    }
}
