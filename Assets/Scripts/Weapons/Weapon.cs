using StarterAssets;
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
    bool hasChildren; 
    protected bool weaponReady;
    public InventoryItem WeaponData { get { return m_WeaponData; } }
    Transform[] children;

    protected StarterAssetsInputs inputs;
    protected virtual void Start()
    {
        if(m_WeaponData != null)
        {
            SetupWeaponData(m_WeaponData);
        }
        originalLayer = LayerMask.LayerToName(gameObject.layer);
        hasChildren = transform.childCount > 0;
        if(hasChildren)
            children = transform.GetComponentsInChildren<Transform>();
    }
    public void SetWeaponReady()
    {
        weaponReady = true;
    }

    protected virtual void Update()
    {
        if (resetTimer <= 0.0f)
        {
            m_BeingRaycasted = false;
            // A perfect place to remove this weapon from shared location in memory.
            WeaponsSingleton.Instance.RemoveWeaponFromMemory(this);
            if(hasChildren)
            {
                foreach (Transform t in children)
                    t.gameObject.layer = LayerMask.NameToLayer(originalLayer);
            }
            else 
            {
                gameObject.layer = LayerMask.NameToLayer(originalLayer);
            }
            resetTimer = m_ResetTimer;
        }
        //switch (weaponType)
        //{
        //    case InventoryItemType.Melee:
        //        // Apply melee animation, thats all and run the cool down timer.
        //        break;
        //    case InventoryItemType.FireArm:
        //        //This needs a cool down timer and this should check if its a single fire or automatic
        //        break;
        //    default:
        //        Debug.LogWarning("Not a weapon");
        //        break;
        //}
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
            if(hasChildren)
            {
                foreach (Transform child in children)
                {
                    child.gameObject.layer = LayerMask.NameToLayer(targetLayer);
                }
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer(targetLayer);
            }
            m_BeingRaycasted = true;
            resetTimer = m_ResetTimer;
            // we need to push this gameobject into a shared location on memory so other systems can access it when needed and remove it when not in use
            // Overwrite the existing weapon if newer.
            WeaponsSingleton.Instance.SetWeaponForPickup(this);
        }
    }
    public void SetupWeaponData(InventoryItem weaponData)
    {
        weaponType = weaponData.ItemType;
        weaponName = weaponData.InventoryItemName;
        weaponDescription = weaponData.InventoryItemDescription;
    }
    public void OnPicked(StarterAssetsInputs inputs)
    {
        this.inputs = inputs;
    }
    public void OnDropped()
    {
        this.inputs = null;
    }
}
