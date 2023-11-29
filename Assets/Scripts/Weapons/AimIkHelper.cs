using StarterAssets;
using System;
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
    [SerializeField] SharedBoolVariable m_Reload;
    StarterAssetsInputs inputs;
    bool requiresLeftHandIKTarget;
    FullyAutomaticWeapon automaticWeapon;
    Shotgun shotgun;
    Burst burst;
    BoltAction boltAction;
    RaycastHit hit;
    Camera mainCamera;
    Vector3 aimPosition;
    bool inReload;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        WeaponsSingleton.Instance.ReloadComplete += onReload;
        inputs = GetComponent<StarterAssetsInputs>();
    }

    private void onReload()
    {
        inReload = false;
        m_LeftHandIkRig.weight = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Reload.Value)
        {
            m_LeftHandIkRig.weight = 0.0f;
            m_HeadIK.weight = 0.0f;
            m_SpineIK.weight = 0f;
            m_RigToEnable.weight = 0.0f;
            inReload = true;
            return;
        }
        if(inputs.Reload)
        {
            inReload = true;
        }
        requiresLeftHandIKTarget = WeaponsSingleton.Instance.ArmedWeapon != null && WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ItemType == InventoryItemType.FireArm && WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ShotConfigration.HandlingType == HandlingType.DualHand;
        if (requiresLeftHandIKTarget && !inReload)
        {
            switch(WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ShotConfigration.ShotType)
            {
                case ShotType.BoltAction:
                    boltAction = (BoltAction)WeaponsSingleton.Instance.ArmedWeapon;
                    m_LeftHandIkTarget.position = boltAction.LeftHandIkTransform.position;
                    m_LeftHandIkTarget.localRotation = boltAction.LeftHandIkTransform.localRotation;
                    m_LeftHandIkRig.weight = 1.0f;
                    break;
                case ShotType.FullyAuto:
                    automaticWeapon = (FullyAutomaticWeapon)WeaponsSingleton.Instance.ArmedWeapon;
                    m_LeftHandIkTarget.position = automaticWeapon.LeftHandIkTransform.position;
                    m_LeftHandIkTarget.localRotation = automaticWeapon.LeftHandIkTransform.localRotation;
                    m_LeftHandIkRig.weight = 1.0f;
                    break;
                case ShotType.SemiAuto:
                    shotgun = (Shotgun)WeaponsSingleton.Instance.ArmedWeapon;
                    m_LeftHandIkTarget.position = shotgun.LeftHandIkTransform.position;
                    m_LeftHandIkTarget.localRotation = shotgun.LeftHandIkTransform.localRotation;
                    m_LeftHandIkRig.weight = 1.0f;
                    break;
                case ShotType.Burst:
                    burst = (Burst)WeaponsSingleton.Instance.ArmedWeapon;
                    m_LeftHandIkTarget.position = burst.LeftHandIkTransform.position;
                    m_LeftHandIkTarget.localRotation = burst.LeftHandIkTransform.localRotation;
                    m_LeftHandIkRig.weight = 1.0f;
                    break;
                default:
                    m_LeftHandIkRig.weight = 0.0f;
                    break;
            }
            
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
