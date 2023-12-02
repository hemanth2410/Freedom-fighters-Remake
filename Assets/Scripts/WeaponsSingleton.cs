using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
// Test commit
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
    Weapon armedWeapon;
    IObjectPool<GameObject> bulletPool;
    IObjectPool<GameObject> decalPool;
    IObjectPool<GameObject> bloodPool;
    Recoil recoilProcessor;
    public Weapon ArmedWeapon { get { return armedWeapon; } }
    public Weapon WeaponToShare { get { return weaponToShare; } }
    /// <summary>
    /// This will be depricated soon
    /// </summary>
    public event Action<InventoryItem> WeaponPicked;
    public event Action<InventoryItem, bool> WeaponAdded;
    public event Action<InventoryItem> DropWeapon;
    public event Action<int> SwitchWeapon;
    public event Action ReloadComplete;
    public IObjectPool<GameObject> BulletPool { get { return bulletPool; } }
    public IObjectPool<GameObject> DecalPool { get { return decalPool; } }
    public IObjectPool<GameObject> BloodPool { get { return bloodPool; } }
    public Recoil RecoilProcessor { get {  return recoilProcessor; } }
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

    public void InvokeWeaponAdded(InventoryItem weaponInventoryItem, bool destroyObject)
    {
        WeaponAdded?.Invoke(weaponInventoryItem, destroyObject);
    }
    public void InvokeDropWeapon(InventoryItem weaponInventoryItem)
    {
        DropWeapon?.Invoke(weaponInventoryItem);
    }
    public void InvokeWeaponSwitch(int value)
    {
        SwitchWeapon?.Invoke(value);
    }

    public void SetArmedWeapon(Weapon weapon)
    {
        armedWeapon = weapon;
    }

    public void RegisterBulletPool(IObjectPool<GameObject> pool)
    {
        bulletPool = pool;
    }
    public void RegisterDecalPool(IObjectPool<GameObject> pool)
    {
        decalPool = pool;
    }
    public void RegisterBloodPool(IObjectPool<GameObject> pool)
    {
        bloodPool = pool;
    }
    public void RegisterRecoilProcessor(Recoil recoil)
    {
        recoilProcessor = recoil;
    }

    public void InvokeReloadComplete()
    {
        ReloadComplete?.Invoke();
    }
}
