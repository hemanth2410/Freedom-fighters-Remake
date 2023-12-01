using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCameraToggle : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_aimCamera;
    [SerializeField] CinemachineVirtualCamera m_sniperCamera;
    [SerializeField] Transform m_variableTransform;
    [SerializeField] SharedVector3Variable m_CameraPosition;
    [SerializeField] SharedVector3Variable m_CameraRotation;
    [SerializeField] SharedBoolVariable m_toggleAimVariable;
    [SerializeField] SharedBoolVariable m_sniperSharedBool;
    // Update is called once per frame
    void Update()
    {
        m_aimCamera.Priority = m_toggleAimVariable.Value ? 20 : 0;
        m_sniperCamera.Priority = m_toggleAimVariable.Value && m_sniperSharedBool.Value ? 40 : 0;
        if(m_sniperSharedBool.Value)
        {
            m_variableTransform.position = m_CameraPosition.Value;
            //m_variableTransform.rotation = Quaternion.Euler(m_CameraRotation.Value);
        }
    }
}
