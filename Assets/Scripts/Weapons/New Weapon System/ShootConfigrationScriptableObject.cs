using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Shoot Config", menuName = "Guns/Shoot Configration", order = 2)]
public class ShootConfigrationScriptableObject : ScriptableObject
{
    [SerializeField] LayerMask m_HitMask;
    [SerializeField] float m_spread;
    [SerializeField][Tooltip("Write the value in Rounds per minute, this class will convert rounds per minute to seconds per shot")] int m_Firerate;

    public LayerMask HitMask { get { return m_HitMask; } }
    public float spread { get { return m_spread; } }
    public float FireRate { get { return (60.0f/m_Firerate); } }
}
