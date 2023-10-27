using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSingleton : MonoBehaviour
{
    static WeaponsSingleton instance;
    public static WeaponsSingleton Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<WeaponsSingleton>();
            }
            return instance;
        }
    }

    Weapon weaponToShare;

    public Weapon WeaponToShare { get { return weaponToShare; } }

    public event Action<InventoryItem> WeaponPicked;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != this)
        {
            Destroy(this);
        }
    }
    public void SetWeaponForPickup(Weapon weaponToPickup)
    {
        weaponToShare = weaponToPickup;
    }
    public void RemoveWeaponFromMemory(Weapon weapon)
    {
        if(weaponToShare == weapon)
        {
            weaponToShare = null;
        }
    }
    public void InvokeWeaponPicked(InventoryItem weaponInventoryItem)
    {
        WeaponPicked?.Invoke(weaponInventoryItem);
    }
}
