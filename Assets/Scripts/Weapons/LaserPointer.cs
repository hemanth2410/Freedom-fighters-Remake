using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] SharedVector3Variable m_AimPositionVariable;
    [SerializeField] Transform m_LaserStartTransform;
    [SerializeField] SharedBoolVariable m_AimBool;
    LineRenderer linerenderer;
    bool enablelaser;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponentInChildren<LineRenderer>();
        enablelaser = GetComponentInParent<ThirdpersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        linerenderer.enabled = m_AimBool.Value && enablelaser;
        if(m_AimPositionVariable != null && m_AimBool != null && m_AimBool.Value)
        {
            linerenderer.SetPosition(0, m_LaserStartTransform.position);
            linerenderer.SetPosition(1, m_AimPositionVariable.Value);
        }
    }
}
