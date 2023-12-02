using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class FullyAutomaticWeapon : Weapon
{
    [SerializeField] Transform m_LeftHandIkTransform;
    public Transform LeftHandIkTransform { get { return m_LeftHandIkTransform; } }

    [Header("Weapon properties")]
    [SerializeField] int m_FireRate;
    [SerializeField] float m_MuzzleVelosity;
    [SerializeField] float m_MaxSpread;
    [SerializeField] Transform m_FireTransform;
    [SerializeField] AudioSource m_FirstShot;
    [SerializeField] AudioSource m_Loop;
    [SerializeField] AudioSource m_Far;
    [SerializeField] ParticleSystem fireSystem;
    [SerializeField] ParticleSystem shells;
    [SerializeField] SharedBoolVariable m_ReloadReticle;
    float timeBetweenShots;
    float lastshotTime;
    float spreadFactor;
    CinemachineImpulseSource impulseSource;
    WeaponAudioSystem audioSystem;
    int currentMagCapacity;
    bool trailPlayed;
    protected override void Start()
    {
        base.Start();
        if (!weaponReady)
            return;
        timeBetweenShots = 60.0f / m_FireRate;
        //fireSystem = GetComponentInChildren<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        WeaponsSingleton.Instance.RecoilProcessor.SetupTextureRecoil(WeaponData.ShotConfigration.RecoilTexture);
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        WeaponsSingleton.Instance.ReloadComplete += onReload;
        audioSystem = GetComponentInChildren<WeaponAudioSystem>();
        trailPlayed = true;
    }

    void onReload()
    {
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        m_ReloadReticle.SetValue(false);
    }
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
            audioSystem.StopLoopAudio();
            //audioSystem.PlayTrailSound(WeaponData.AudioConfigration.Trail);
            // set some value to reload reticle???
            return;
        }
        if (inputs != null && inputs.Attack && Time.time - lastshotTime > timeBetweenShots)
        {

            if (!fireSystem.isPlaying)
            {
                fireSystem.Play();
                shells.Play();
                audioSystem.PlayShotAudio(WeaponData.AudioConfigration.NearClip);
                audioSystem.playFarAudio(WeaponData.AudioConfigration.FarClip);
                audioSystem.playLoopSound(WeaponData.AudioConfigration.Loop);
                trailPlayed = true;
            }
            lastshotTime = Time.time;
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);
            spreadFactor += Time.deltaTime;
            spreadFactor = Mathf.Clamp(spreadFactor, 0.0f, m_MaxSpread);
            switch (WeaponData.ShotConfigration.RecoilType)
            {
                case RecoilType.None:
                    break;
                case RecoilType.Simple:
                    bullet.transform.forward = m_FireTransform.forward;
                    bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.28f, spreadFactor);
                    bullet.GetComponent<Bullet>().SetDamage(40);
                    break;
                case RecoilType.Texture:
                    bullet.transform.forward = m_FireTransform.forward;
                    Vector3 _recoil = WeaponsSingleton.Instance.RecoilProcessor.GetRecoil(spreadFactor / m_MaxSpread).normalized;
                    bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, _recoil * 0.01f, true, 0.28f);
                    bullet.GetComponent<Bullet>().SetDamage(40);
                    break;
            }
            //bullet.transform.forward = m_FireTransform.forward;
            //bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, true, 0.28f,spreadFactor);
            impulseSource.GenerateImpulse();
            currentMagCapacity--;
            if (currentMagCapacity == 0)
                audioSystem.PlayTrailSound(WeaponData.AudioConfigration.Trail);
        }
        if (inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            audioSystem.StopLoopAudio();
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
            //audioSystem.PlayTrailSound(WeaponData.AudioConfigration.Trail);
        }
        //if (!inputs.Attack && trailPlayed)
        //{
        //    trailPlayed = false;
        //    audioSystem.PlayTrailSound(WeaponData.AudioConfigration.Trail);
        //}
    }
}
