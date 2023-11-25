using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedBoolUiHelper : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_SharedBool;
    [SerializeField] SharedBoolVariable m_ReloadReticleBool;
    [SerializeField] bool m_Negate;
    [SerializeField] GameObject m_TargetObject;
    [SerializeField] GameObject m_ReloadReticle;
    // Update is called once per frame
    void Update()
    {
        m_TargetObject.SetActive(m_Negate ? !m_SharedBool.Value : m_SharedBool.Value);
        m_ReloadReticle.SetActive(m_ReloadReticleBool.Value);
    }
}
