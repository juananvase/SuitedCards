using System;
using System.Collections;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            //Attack();
        }
    }
}
