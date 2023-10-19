using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCameraToggle : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_aimCamera;
    [SerializeField] SharedBoolVariable m_toggleAimVariable;
    // Update is called once per frame
    void Update()
    {
        m_aimCamera.Priority = m_toggleAimVariable.Value ? 20 : 0;
    }
}
