using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventoryStarterTemplate m_StarterTemplate;
    //[SerializeField] List<InventoryItem> playerInventoryItems = new List<InventoryItem>();
    [SerializeField] SharedBoolVariable m_PickupReady;
    InventoryUI inventoryUI;
    StarterAssetsInputs inputs;
    [SerializeField] Inventory m_PlayerInventory;
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
                    break;
                case InventoryItemType.Melee:
                    m_PlayerInventory.AddMeleeWeapon(WeaponsSingleton.Instance.WeaponToShare.WeaponData);
                    break;
                case InventoryItemType.Throwable: 
                    break;
                case InventoryItemType.Consumable: 
                    break;
            }
        }
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
    public void InitializeInventory(int numberOftrowablesPerSlot, int maxNumberOfHelables)
    {
        remainingHelables = numberOftrowablesPerSlot;
    }
    public void AddMeleeWeapon(InventoryItem item)
    {
        // destroy already armed game object.
        // add this as new gameobject to the right hand.
        MeleeWeapon = item;
        // Now invoke weapon replaced event so drop the inventory item or destroy it.
        WeaponsSingleton.Instance.InvokeWeaponPicked(item);
    }
}