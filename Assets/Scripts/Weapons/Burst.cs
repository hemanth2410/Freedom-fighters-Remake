using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform { get { return m_LeftHandIkTransform; } }
    [Header("Weapon properties")]
    [SerializeField] int m_FireRate;
    [SerializeField] int m_bulletsPerBurst;
    [SerializeField] float m_MuzzleVelosity;
    [SerializeField] float m_MaxSpread;
    [SerializeField] Transform m_FireTransform;
    [SerializeField] ParticleSystem fireSystem;
    [SerializeField] ParticleSystem shells;
    [SerializeField] SharedBoolVariable m_ReloadReticle;
    float timeBetweenShots;
    float lastshotTime;
    float spreadFactor;
    CinemachineImpulseSource impulseSource;
    int currentMagCapacity;
    bool weaponLocked;
    int shotsRemaining;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!weaponReady)
            return;
        timeBetweenShots = 60.0f / m_FireRate;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        WeaponsSingleton.Instance.ReloadComplete += onReload;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        WeaponsSingleton.Instance.RecoilProcessor.SetupTextureRecoil(WeaponData.ShotConfigration.RecoilTexture);
    }

    private void onReload()
    {
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        m_ReloadReticle.SetValue(false);
        weaponLocked = false;
        shotsRemaining = m_bulletsPerBurst;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!weaponReady)
            return;
        if (currentMagCapacity <= 0)
        {
            m_ReloadReticle.SetValue(true);
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
            // set some value to reload reticle???
            return;
        }
        if (inputs.Attack && !weaponLocked)
        {
            weaponLocked = true;
            shells.Play();
            fireSystem.Play();
            //lastshotTime = Time.time;
            shotsRemaining = m_bulletsPerBurst;

        }
        if(weaponLocked && Time.time - lastshotTime > timeBetweenShots && shotsRemaining > 0)
        {
            lastshotTime = Time.time;
            shotsRemaining--;
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);
            spreadFactor += Time.deltaTime;
            spreadFactor = Mathf.Clamp(spreadFactor, 0.0f, m_MaxSpread);
            bullet.transform.forward = m_FireTransform.forward;
            bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.28f, spreadFactor);
            currentMagCapacity--;
            impulseSource.GenerateImpulse();
        }
        if(shotsRemaining <= 0)
        {
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
        }
        if (inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            weaponLocked = false;
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
            shotsRemaining = m_bulletsPerBurst;
        }
    }
}
