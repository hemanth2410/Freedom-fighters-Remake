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
    [SerializeField] Rig m_LeftHandIkRig;
    [SerializeField] Rig m_SpineIK;
    [SerializeField] Rig m_HeadIK;
    [SerializeField] Transform m_LeftHandIkTarget;
    [SerializeField] SharedBoolVariable m_AimSharedBoolVariable;
    [SerializeField] SharedVector3Variable m_AimSharedVector3;
    [SerializeField] LayerMask m_AimLayers;
    bool requiresLeftHandIKTarget;
    FullyAutomaticWeapon automaticWeapon;
    RaycastHit hit;
    Camera mainCamera;
    Vector3 aimPosition;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        requiresLeftHandIKTarget = WeaponsSingleton.Instance.ArmedWeapon != null && WeaponsSingleton.Instance.ArmedWeapon.WeaponData.HandlingType == HandlingType.DualHand;
        if (requiresLeftHandIKTarget)
        {
            automaticWeapon = (FullyAutomaticWeapon)WeaponsSingleton.Instance.ArmedWeapon;
            m_LeftHandIkTarget.position = automaticWeapon.LeftHandIkTransform.position;
            m_LeftHandIkTarget.localRotation = automaticWeapon.LeftHandIkTransform.localRotation;
            m_LeftHandIkRig.weight = 1.0f;
        }
        else
        {
            m_LeftHandIkRig.weight = 0.0f;
        }
        if(m_AimSharedBoolVariable.Value)
        {
            m_TransformToMove.position = m_AimSharedVector3.Value;
            m_RigToEnable.weight = 1.0f;
            m_SpineIK.weight = 1.0f;
            m_HeadIK.weight = 1.0f;
        }
        else
        {
            m_RigToEnable.weight = 0.0f;
            m_SpineIK.weight = 0.0f;
            m_HeadIK.weight = 0.0f;
        }
    }
}
