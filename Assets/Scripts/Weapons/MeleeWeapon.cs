using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] float m_DamagePerSecond;
    // later we need to get enemy layers from constants.

    private void OnTriggerStay(Collider other)
    {
        //if(enemy)
        // enemy.TakeDamage(m_DamagePerSecond * Time.DeltaTime);
    }
}
