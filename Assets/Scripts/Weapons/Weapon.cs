using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Weapon : MonoBehaviour
{
    [SerializeField] InventoryItem m_WeaponData;
    [SerializeField] float m_ResetTimer;
    [SerializeField] bool m_BeingRaycasted;
    private InventoryItemType weaponType;
    private string weaponName;
    private string weaponDescription;
    private string originalLayer;
    float resetTimer;
    private LayerMask targetLayer;
    private void Start()
    {
        if(m_WeaponData != null)
        {
            SetupWeaponData(m_WeaponData);
        }
        originalLayer = LayerMask.LayerToName(gameObject.layer);
    }

    private void Update()
    {
        if (resetTimer <= 0.0f)
        {
            m_BeingRaycasted = false;
            gameObject.layer = LayerMask.NameToLayer(originalLayer);
            resetTimer = m_ResetTimer;
        }
        switch (weaponType)
        {
            case InventoryItemType.Melee:
                // Apply melee animation, thats all and run the cool down timer.
                break;
            case InventoryItemType.FireArm:
                //This needs a cool down timer and this should check if its a single fire or automatic
                break;
            default:
                Debug.LogWarning("Not a weapon");
                break;
        }
        if(resetTimer > 0.0f)
        {
            resetTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        // Keep refreshing weapon layer Constantly
        
    }
    public void UpdateWeaponLayer(string targetLayer)
    {
        this.targetLayer = LayerMask.NameToLayer(targetLayer);
        if (gameObject.layer != LayerMask.NameToLayer(targetLayer))
        {
            gameObject.layer = LayerMask.NameToLayer(targetLayer);
            m_BeingRaycasted = true;
            resetTimer = m_ResetTimer;
        }
    }
    public void SetupWeaponData(InventoryItem weaponData)
    {
        weaponType = weaponData.ItemType;
        weaponName = weaponData.InventoryItemName;
        weaponDescription = weaponData.InventoryItemDescription;
    }
}