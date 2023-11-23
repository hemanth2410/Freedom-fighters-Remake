using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullyAutomaticWeapon : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform {  get { return m_LeftHandIkTransform; } }

    [Header("Weapon properties")]
    [SerializeField] int m_FireRate;
    [SerializeField] float m_MuzzleVelosity;
    [SerializeField] Transform m_FireTransform;
    float timeBetweenShots;
    float lastshotTime;
    protected override void Start()
    {
        base.Start();
        timeBetweenShots = 60.0f / m_FireRate;
    }
    protected override void Update()
    {
        base.Update();
        if(inputs != null && inputs.Attack &&  Time.time - lastshotTime > timeBetweenShots)
        {
            lastshotTime = Time.time;
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.28f);
            bullet.transform.forward = m_FireTransform.forward;
        }
    }
}
