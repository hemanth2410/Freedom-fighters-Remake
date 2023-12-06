using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoWeapons : Weapon
{
    [Header("Weapon properties")]
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
    WeaponAudioSystem audioSystem;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!weaponReady)
            return;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        currentMagCapacity = WeaponData.ShotConfigration.MagCapacity;
        WeaponsSingleton.Instance.ReloadComplete += onReload;
        timeBetweenShots = 0.5f;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        WeaponsSingleton.Instance.RecoilProcessor.SetupTextureRecoil(WeaponData.ShotConfigration.RecoilTexture);
        audioSystem = GetComponentInChildren<WeaponAudioSystem>();
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
        if (inputs.Attack && Time.time - lastshotTime > timeBetweenShots)
        {
            shells.Play();
            fireSystem.Play();
            lastshotTime = Time.time;
            currentMagCapacity--;
            var bullet = WeaponsSingleton.Instance.BulletPool.Get();
            bullet.SetActive(true);
            bullet.transform.forward = m_FireTransform.forward;
            //bullet.transform.forward += transform.TransformDirection(offset);
            bullet.GetComponent<Bullet>().SetupBullet(m_MuzzleVelosity, m_FireTransform.position, 3.0f, false, 0.28f, 0);
            bullet.GetComponent<Bullet>().SetDamage(40);
            impulseSource.GenerateImpulse();
            audioSystem.PlayShotAudio(WeaponData.AudioConfigration.NearClip);
            audioSystem.PlayShotAudio(WeaponData.AudioConfigration.FarClip);
        }
        if (inputs != null && !inputs.Attack && !fireSystem.isStopped)
        {
            fireSystem.Stop();
            shells.Stop();
            spreadFactor = 0.0f;
        }
    }
}
