using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventoryStarterTemplate m_StarterTemplate;
    //[SerializeField] List<InventoryItem> playerInventoryItems = new List<InventoryItem>();
    [SerializeField] SharedBoolVariable m_PickupReady;
    InventoryUI inventoryUI;
    StarterAssetsInputs inputs;
    [SerializeField] Inventory m_PlayerInventory;

    public Inventory Inventory { get { return m_PlayerInventory; } }
    // Start is called before the first frame update

    // We need to set some rules for Inventory system
    // Weapon and its ammo needs to be in 1 place
    // Throwables 3 should have 3 slots Molotov, Grenades and ClayMores?
    // One slot for all the Helables
    // So maximum amount of Helables = 8 and player can carry upto 6 grenades, 6 molotov, 6 claymores.
    // Ammo capacity depends on type of ammunation (we may need to define it in Weapon class or some other place where it makes sense)
    // A slot of melee weapon
    // 2 wepons + 1 Helables + 3 Throwables + 1 melee => 7 different inventory slots.

    // Having a different structure for Holding player Data is a good idea here.

    void Start()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        inputs = GetComponent<StarterAssetsInputs>();
        if(m_StarterTemplate != null)
        {
            // here we will have to load them into a shared variable to avoid cyclic dependancies.
        }
        m_PlayerInventory.InitializeInventory(8, 8);
        inventoryUI.ConstructInventoryUI();

        // and we need to call this method when we are changing or removing Inventory Items to re construct UI
    }
    // Update is called once per frame
    void Update()
    {
        // Handle Inpyt logic and pickup logic
        // Just like reticle we use a shared variable to display
        // The UI of Inventory. A shared boolean.
        if(inputs.Pickup && m_PickupReady.Value)
        {
            // pickup the weapon
            //addInventoryItemToList(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
            //WeaponsSingleton.Instance.InvokeWeaponPicked(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
            switch(WeaponsSingleton.Instance.WeaponToShare.WeaponData.ItemType)
            {
                case InventoryItemType.FireArm:
                    // A perfect place to add weapon to pistol inventory
                    if(WeaponsSingleton.Instance.WeaponToShare.WeaponData.ShotConfigration.HandlingType == HandlingType.SingleHand)
                    {
                        m_PlayerInventory.AddSecondaryWeapon(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
                    }
                    if(WeaponsSingleton.Instance.WeaponToShare.WeaponData.ShotConfigration.HandlingType == HandlingType.DualHand)
                    {
                        Debug.Log("Adding primary weapon");
                        m_PlayerInventory.AddPrimaryWeapon(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
                    }
                    break;
                case InventoryItemType.Melee:
                    m_PlayerInventory.AddMeleeWeapon(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
                    break;
                case InventoryItemType.Throwable:
                    var pickupItem = WeaponsSingleton.Instance.WeaponToShare.WeaponData;
                    if(pickupItem.InventoryItemPrefab.GetComponent<GrenadeThrowable>())
                    {
                        m_PlayerInventory.AddGrenade(pickupItem);
                    }
                    if (pickupItem.InventoryItemPrefab.GetComponent<SmokeThrowable>())
                    {
                        m_PlayerInventory.AddSmoke(pickupItem);
                    }
                    if(pickupItem.InventoryItemPrefab.GetComponent<FlashbangThrowable>())
                    {
                        m_PlayerInventory.AddFlashbang(pickupItem);
                    }
                    // type cast throwables and store them for visual representation
                    break;
                case InventoryItemType.Consumable: 
                    break;
            }
        }
        // add weapons switch ligic here
        // 1. for primary weapon
        // 2. for secondary weapon
        // 3. for melee weapon
        // Scrolling will only switch between primary and secondary
    }
}
[System.Serializable]
public struct Inventory
{
    public InventoryItem MeleeWeapon;
    public InventoryItem Helable;
    public List<InventoryItem> Throwables;
    public InventoryItem PrimaryFireArm;
    public InventoryItem SecondaryFireArm;
    int remainingHelables;
    public List<InventoryItem> usableWeapons;
    public int RemainingGrenades;
    public int RemainingFlashBangs;
    public int RemainingSmoke;
    public void InitializeInventory(int numberOftrowablesPerSlot, int maxNumberOfHelables)
    {
        remainingHelables = numberOftrowablesPerSlot;
        usableWeapons = new List<InventoryItem>();
    }
    public void AddMeleeWeapon(InventoryItem item)
    {
        // destroy already armed game object.
        // add this as new gameobject to the right hand.
        if(MeleeWeapon != null)
        {
            WeaponsSingleton.Instance.InvokeDropWeapon(MeleeWeapon);
            usableWeapons.Remove(MeleeWeapon);
        }
        MeleeWeapon = item;
        // Now invoke weapon replaced event so drop the inventory item or destroy it.
        WeaponsSingleton.Instance.InvokeWeaponPicked(item);
        usableWeapons.Add(item);
    }
    public void AddPrimaryWeapon(InventoryItem item) 
    { 
        if(PrimaryFireArm != null)
        {
            WeaponsSingleton.Instance.InvokeDropWeapon(PrimaryFireArm);
            usableWeapons.Remove(PrimaryFireArm);
        }
        PrimaryFireArm = item;
        WeaponsSingleton.Instance.InvokeWeaponPicked(item);
        usableWeapons.Add(item);
    }
    public void AddSecondaryWeapon(InventoryItem item)
    {
        if(SecondaryFireArm != null)
        {
            WeaponsSingleton.Instance.InvokeDropWeapon(SecondaryFireArm);
            usableWeapons.Remove(SecondaryFireArm);
        }
        SecondaryFireArm = item;
        WeaponsSingleton.Instance.InvokeWeaponPicked(item);
        usableWeapons.Add(item);
    }
    public void RemoveDroppedWeapon(InventoryItem item)
    {
        usableWeapons.Remove(item);
    }

    public void AddGrenade(InventoryItem grenade)
    {
        if(!usableWeapons.Any(x => (GrenadeThrowable)x.InventoryItemPrefab.GetComponent<Weapon>()))
        {
            WeaponsSingleton.Instance.InvokeWeaponPicked(grenade);
            usableWeapons.Add(grenade);
        }
        RemainingGrenades++;
    }
    public void AddFlashbang(InventoryItem flashbang)
    {
        if (!usableWeapons.Any(x => (FlashbangThrowable)x.InventoryItemPrefab.GetComponent<Weapon>()))
        {
            WeaponsSingleton.Instance.InvokeWeaponPicked(flashbang);
            usableWeapons.Add(flashbang);
        }
        RemainingFlashBangs++;
    }
    public void AddSmoke(InventoryItem smoke)
    {
        if (!usableWeapons.Any(x => (SmokeThrowable)x.InventoryItemPrefab.GetComponent<Weapon>()))
        {
            WeaponsSingleton.Instance.InvokeWeaponPicked(smoke);
            usableWeapons.Add(smoke);
        }
        RemainingSmoke++;
    }
    public void RemoveGrenade()
    {
        RemainingGrenades--;
    }
}