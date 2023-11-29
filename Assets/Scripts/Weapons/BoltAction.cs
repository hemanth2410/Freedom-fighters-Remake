using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltAction : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform { get { return m_LeftHandIkTransform; } }
    [Header("Weapon properties")]
    [SerializeField] int m_FireRate;
    [SerializeField] float m_MuzzleVelosity;
    [SerializeField] Transform m_FireTransform;
    [SerializeField] ParticleSystem fireSystem;
    [SerializeField] ParticleSystem shells;
    [SerializeField] ParticleSystem snierLine; 
    [SerializeField] SharedBoolVariable m_ReloadReticle;
    float timeBetweenShots;
    float lastshotTime;
    float spreadFactor;
    CinemachineImpulseSource impulseSource;
    int currentMagCapacity;
    bool weaponLocked;
    WeaponAudioSystem weaponAudioSystem;
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
        WeaponsSingleton.Instance.RecoilProcessor.SetupTextureRecoil(WeaponData.ShotConfigration.RecoilTexture);
        weaponAudioSystem = GetComponentInChildren<WeaponAudioSystem>();
    }

    private void onReload()
    {
        //throw new NotImplementedException();
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        m_ReloadReticle.SetValue(false);
        weaponLocked = false;
    }
    void nextShotReady()
    {
        weaponLocked = false;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!weaponReady)
            return;
        if (currentMagCapacity <= 0)
        {
            weaponLocked = false;
            m_ReloadReticle.SetValue(true);
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
            // set some value to reload reticle???
            return;
        }
        if (inputs.Attack && Time.time - lastshotTime > timeBetweenShots && !weaponLocked)
        {
            shells.Play();
            fireSystem.Play();
            snierLine.Play();
            lastshotTime = Time.time;
            currentMagCapacity--;
            impulseSource.GenerateImpulse();
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);
            bullet.transform.forward = m_FireTransform.forward;
            bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.4f, 0.0f);
            weaponAudioSystem.PlayShotAudio(WeaponData.AudioConfigration.NearClip);
            weaponAudioSystem.PlayShotAudio(WeaponData.AudioConfigration.FarClip);
            weaponLocked = true;
        }
        if (inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
        }
    }
}
