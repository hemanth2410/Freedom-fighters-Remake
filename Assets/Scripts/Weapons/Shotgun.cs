using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform { get { return m_LeftHandIkTransform; } }
    [Header("Weapon properties")]
    [SerializeField] int m_bulletsPerShot;
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
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        WeaponsSingleton.Instance.RecoilProcessor.SetupTextureRecoil(WeaponData.ShotConfigration.RecoilTexture);
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        WeaponsSingleton.Instance.ReloadComplete += onReload;
        timeBetweenShots = 1.2f;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void onReload()
    {
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        m_ReloadReticle.SetValue(false);
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
        if(inputs.Attack && Time.time - lastshotTime > timeBetweenShots)
        {
            lastshotTime = Time.time;
            currentMagCapacity--;
            float angle = 360.0f / m_bulletsPerShot;
            for (int i = 0; i < m_bulletsPerShot; i++)
            {
                var bullet = WeaponsSingleton.Instance.BulletPool.Get();
                bullet.SetActive(true);
                Vector3 offset = new Vector3(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0);
                bullet.transform.forward = m_FireTransform.forward + offset * m_MaxSpread * m_MaxSpread;
                //bullet.transform.forward += transform.TransformDirection(offset);
                bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, false, 0.28f, 0);
            }
            impulseSource.GenerateImpulse();
        }
        if (inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;

        }
    }
}
