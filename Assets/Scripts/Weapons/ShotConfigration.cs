using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shot configration", menuName = "Inventory System/Shot configration")]
public class ShotConfigration : ScriptableObject
{
    [SerializeField] ShotType m_shotType;
    [SerializeField] HandlingType m_handlingType;
    [SerializeField] RecoilType m_recoilType;
    [SerializeField] float m_maxSpread;
    [SerializeField] Texture2D m_recoilTexture;
    [SerializeField]
    [Tooltip("We typically want IK for two handed and single handed projectile weapons")]
    bool m_RequiresIK = false;
    [SerializeField] bool m_supportsSecondaryFireMode;
    [SerializeField] ShotType m_SecondaryShotType;
    [SerializeField] int m_magCapacity;


    public HandlingType HandlingType { get { return m_handlingType; } }
    public ShotType ShotType { get { return m_shotType; } }
    public bool RequiresIK { get { return m_RequiresIK; } }
    public RecoilType RecoilType { get { return m_recoilType;} }
    public float MaxSpread { get { return m_maxSpread; } }
    public Texture2D RecoilTexture {  get { return m_recoilTexture; } }
    public int MagCapacity { get { return m_magCapacity; } }
}
