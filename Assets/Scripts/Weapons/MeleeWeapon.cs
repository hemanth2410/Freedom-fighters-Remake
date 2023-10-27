using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] float m_DamagePerSecond;
    // later we need to get enemy layers from constants.

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<TestSubjectHealth>())
        {
            other.GetComponentInParent<TestSubjectHealth>().SetAnimation("Hit");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<TestSubjectHealth>())
        {
            other.GetComponentInParent<TestSubjectHealth>().TakeDamage(m_DamagePerSecond);
        }
    }
}
