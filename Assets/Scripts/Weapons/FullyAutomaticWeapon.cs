using Cinemachine;
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
    [SerializeField] float m_MaxSpread;
    [SerializeField] Transform m_FireTransform;
    [SerializeField] AudioSource m_FirstShot;
    [SerializeField] AudioSource m_Loop;
    [SerializeField] AudioSource m_Far;
    ParticleSystem fireSystem;
    float timeBetweenShots;
    float lastshotTime;
    float spreadFactor;
    CinemachineImpulseSource impulseSource;
    protected override void Start()
    {
        base.Start();
        timeBetweenShots = 60.0f / m_FireRate;
        fireSystem = GetComponentInChildren<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void Update()
    {
        base.Update();
        
        if(inputs != null && inputs.Attack &&  Time.time - lastshotTime > timeBetweenShots)
        {

            if(!fireSystem.isPlaying)
            {
                fireSystem.Play();
            }
            lastshotTime = Time.time;
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);

            spreadFactor += Time.deltaTime;
            spreadFactor = Mathf.Clamp(spreadFactor, 0.0f, m_MaxSpread);
            bullet.transform.forward = m_FireTransform.forward;
            bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.28f,spreadFactor);
            impulseSource.GenerateImpulse();
        }
        if(inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            fireSystem.Stop();
            spreadFactor = 0.0f;
           
        }
    }
}
