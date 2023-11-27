using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Audio Config", menuName = "Inventory System/Audio Configration", order = 8)]
public class AudioConfigration : ScriptableObject
{
    [SerializeField] AudioClip[] m_NearClips;
    [SerializeField] AudioClip[] m_FarClips;
    [SerializeField] AudioClip m_Loop;
    [SerializeField] AudioClip m_Trail;

    public AudioClip NearClip => m_NearClips[Random.Range(0, m_NearClips.Length - 1)];
    public AudioClip FarClip => m_FarClips[Random.Range(0, m_FarClips.Length - 1)];
    public AudioClip Loop => m_Loop;
    public AudioClip Trail => m_Trail;

}
