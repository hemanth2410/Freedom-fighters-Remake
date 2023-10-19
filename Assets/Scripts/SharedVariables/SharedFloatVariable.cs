using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New shared float variable", menuName = "Shared Variables/new shared float")]
public class SharedFloatVariable : ScriptableObject
{
    [SerializeField] private float m_Value;
    public float Value { get { return m_Value; } }

    public void SetValue(float value)
    {
        m_Value = value;
    }
}
