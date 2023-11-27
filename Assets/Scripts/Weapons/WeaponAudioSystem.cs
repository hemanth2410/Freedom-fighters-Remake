using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource m_ShotAudioSource;
    [SerializeField] AudioSource m_FarAudioSource;
    [SerializeField] AudioSource m_TrailAudioSource;
    [SerializeField] AudioSource m_LoopAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_LoopAudioSource.loop = true;
        m_ShotAudioSource.playOnAwake = false;
        m_FarAudioSource.playOnAwake = false;
        m_TrailAudioSource.playOnAwake = false;
        m_LoopAudioSource.playOnAwake = false;
    }
    public void PlayShotAudio(AudioClip clip)
    {
        m_ShotAudioSource.clip = clip;
        m_ShotAudioSource.Play();
    }
    public void playFarAudio(AudioClip clip)
    {
        m_FarAudioSource.clip = clip;
        m_FarAudioSource.Play();
    }
    public void playLoopSound(AudioClip clip)
    {
        if(!m_LoopAudioSource.isPlaying)
        {
            m_LoopAudioSource.clip = clip;
            m_LoopAudioSource.Play();
        }
    }
    public void PlayTrailSound(AudioClip clip)
    {
        m_TrailAudioSource.clip = clip;
        m_TrailAudioSource.Play();
    }
    public void StopLoopAudio()
    {
        m_LoopAudioSource.Stop();
    }
}

