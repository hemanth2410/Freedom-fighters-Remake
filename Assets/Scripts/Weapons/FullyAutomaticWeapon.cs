using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullyAutomaticWeapon : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform {  get { return m_LeftHandIkTransform; } }
}
