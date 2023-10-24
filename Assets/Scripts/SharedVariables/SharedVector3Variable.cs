using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New shared Vector3 variable", menuName = "Shared Variables/new shared Vector3")]
public class SharedVector3Variable : ScriptableObject
{
    [SerializeField] private Vector3 m_Value;
    public Vector3 Value { get { return m_Value; } }

    public void SetValue(Vector3 value)
    {
        m_Value = value;
    }
}
