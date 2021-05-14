using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : GeneralTower
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(AttackEnemy());

    }

    protected override void Update()
    {
        base.Update();

    }

    IEnumerator AttackEnemy()
    {
        while (true)
        {
            if (seeEnemy == true)
            {
                Instantiate(bulletType, spawnTransform.position, spawnTransform.rotation);

            }
            
            yield return new WaitForSeconds(waitSpawnBullet);
        }
               
    }
}
