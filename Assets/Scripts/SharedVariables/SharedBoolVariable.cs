using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New shared bool variable", menuName = "Shared Variables/new shared bool")]
public class SharedBoolVariable : ScriptableObject
{
    [SerializeField] private bool m_Value;
    public bool Value { get { return m_Value; } }

    public void SetValue(bool value)
    {
        m_Value = value;
    }
}
