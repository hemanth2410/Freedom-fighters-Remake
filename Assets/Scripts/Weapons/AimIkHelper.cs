using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimIkHelper : MonoBehaviour
{
    [SerializeField] float m_MaxAimDistance;
    [SerializeField] LayerMask m_AimLayerMask;
    [SerializeField] Transform m_TransformToMove;
    [SerializeField] Rig m_RigToEnable;
    [SerializeField] SharedBoolVariable m_AimSharedBoolVariable;
    RaycastHit hit;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_AimSharedBoolVariable.Value)
        {
            m_RigToEnable.weight = 1.0f;
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, m_MaxAimDistance, m_AimLayerMask))
            {
                m_TransformToMove.position = hit.point;
            }
            else
            {
                m_TransformToMove.position = mainCamera.transform.forward * m_MaxAimDistance;
            }
        }
        else
        {
            m_RigToEnable.weight = 0.0f;
        }
    }
}
