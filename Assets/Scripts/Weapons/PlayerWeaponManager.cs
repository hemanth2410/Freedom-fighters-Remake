using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField][Tooltip("Override this value if Root bone is of different scale")] float m_weaponScaleMultiplier = 1.0f;
    [SerializeField] Transform m_handBone;
    [SerializeField] float m_coolDown;
    [SerializeField] Transform m_DropTransform;
    [SerializeField] SharedBoolVariable m_AimSharedBool;
    PlayerInventory playerInventory;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    bool isArmed;
    bool cooldown;
    Weapon armedWeapon;
    float timer;
    int currentIndex = 0;
    Dictionary<string,GameObject> pickedWeapons = new Dictionary<string,GameObject>();

    //public Weapon ArmedWeapon { get { return armedWeapon; } }
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        WeaponsSingleton.Instance.WeaponPicked += Instance_WeaponPicked;
        WeaponsSingleton.Instance.DropWeapon += dropWeapon;
        WeaponsSingleton.Instance.SwitchWeapon += Instance_SwitchWeapon;
    }

    private void Instance_SwitchWeapon(int value)
    {
        if(value == 0 || pickedWeapons.Count == 0)
        {
            return;
        }
        armedWeapon.gameObject.SetActive(false);
        currentIndex += value;
        currentIndex = currentIndex < 0 ? pickedWeapons.Count-1 : currentIndex >= pickedWeapons.Count ? 0 : currentIndex;
        string _requiredWeapon = playerInventory.Inventory.usableWeapons[currentIndex].InventoryItemName;
        armedWeapon.gameObject.SetActive(false);
        armedWeapon = pickedWeapons[_requiredWeapon].GetComponent<Weapon>();
        armedWeapon.gameObject.SetActive(true);
        setArmedWeaponInWeaponsSingleton(armedWeapon);
        isArmed = true;
        if (armedWeapon.WeaponData.HandlingType == HandlingType.DualHand)
        {
            animator.SetBool("TwoHanded", true);
        }
        else
        {
            animator.SetBool("TwoHanded", false);
        }
    }

    private void Instance_WeaponPicked(InventoryItem obj)
    {
        if (isArmed)
        {
            var _j = Instantiate(obj.InventoryItemPrefab, m_handBone);
            _j.transform.localPosition = obj.PositionOffset * m_weaponScaleMultiplier;
            _j.transform.localRotation = Quaternion.Euler(obj.RotationOffset);
            _j.transform.localScale *= m_weaponScaleMultiplier;
            _j.GetComponent<Collider>().isTrigger = true;
            _j.GetComponent<Rigidbody>().isKinematic = true;
            _j.gameObject.layer = LayerMask.NameToLayer("Melee");
            _j.SetActive(false);
            pickedWeapons[obj.InventoryItemName] = _j;
            return;
        }
        var _k = Instantiate(obj.InventoryItemPrefab, m_handBone);
        _k.transform.localPosition = obj.PositionOffset*m_weaponScaleMultiplier;
        _k.transform.localRotation = Quaternion.Euler(obj.RotationOffset);
        _k.transform.localScale *= m_weaponScaleMultiplier;
        _k.GetComponent<Collider>().isTrigger = true;
        _k.GetComponent<Rigidbody>().isKinematic = true;
        _k.gameObject.layer = LayerMask.NameToLayer("Melee");
        armedWeapon = _k.GetComponent<Weapon>();
        setArmedWeaponInWeaponsSingleton(armedWeapon);
        pickedWeapons[obj.InventoryItemName] = _k;
        isArmed = true;
        if(obj.HandlingType == HandlingType.DualHand)
        {
            animator.SetBool("TwoHanded", true);
        }
        else
        {
            animator.SetBool("TwoHanded", false);
        }
        currentIndex = playerInventory.Inventory.usableWeapons.IndexOf(obj);
        
    }

    private void ChangeActiveWeapon(InventoryItem  obj)
    {
        if(isArmed)
        {
            armedWeapon.gameObject.SetActive(false);
            isArmed = false;
            // and again System.Linq saves the day;
            if(playerInventory.Inventory.usableWeapons.Any(x => x.ItemType == obj.ItemType))
            {
                // same item probably destroy and perform cleanup
                dropWeapon(armedWeapon.WeaponData);
                Destroy(armedWeapon.gameObject);
                playerInventory.Inventory.RemoveDroppedWeapon(armedWeapon.WeaponData);
            }
        }
        // Now change the armed weapon
        var _k = Instantiate(obj.InventoryItemPrefab, m_handBone);
        _k.transform.localPosition = obj.PositionOffset*m_weaponScaleMultiplier;
        _k.transform.localRotation = Quaternion.Euler(obj.RotationOffset);
        _k.transform.localScale *= m_weaponScaleMultiplier;
        _k.GetComponent<Collider>().isTrigger = true;
        _k.GetComponent<Rigidbody>().isKinematic = true;
        _k.gameObject.layer = LayerMask.NameToLayer("Melee");
        armedWeapon = _k.GetComponent<Weapon>();
        isArmed = true;
    }

    void dropWeapon(InventoryItem obj)
    {
        if(armedWeapon.WeaponData.ItemType == obj.ItemType)
        {
            Destroy(armedWeapon.gameObject);
            isArmed = false;
        }
        pickedWeapons.Remove(obj.InventoryItemName);
        var k = Instantiate(obj.InventoryItemPrefab, m_DropTransform.position, m_DropTransform.rotation);
        k.GetComponent<Rigidbody>().AddForce(m_DropTransform.forward * 10.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (cooldown)
        {
            timer -= Time.deltaTime;
        }

        animator.SetBool("Aim", m_AimSharedBool.Value);


        // Equip the weapon when the appropriate key is pressed or wheel is scrolled
        if (starterAssetsInputs.Attack && isArmed && !cooldown)
        {
            switch(armedWeapon.WeaponData.ItemType)
            {
                case InventoryItemType.Melee:
                    animator.SetLayerWeight(animator.GetLayerIndex("AttackLayer"), 1.0f);
                    animator.SetInteger("AttackType", Random.Range(0, 10));
                    animator.SetBool("Attack", true);
                    cooldown = true;
                    timer = m_coolDown;
                    break;
                case InventoryItemType.FireArm:
                    break;
                case InventoryItemType.Throwable: 
                    break;
                default: break;
            }
        }
        if (timer < 0.0f)
        {
            cooldown = false;
            animator.SetBool("Attack", false);
            animator.SetLayerWeight(animator.GetLayerIndex("AttackLayer"), 0f);
        }
        else
            return;
    }

    void setArmedWeaponInWeaponsSingleton(Weapon weapon)
    {
        WeaponsSingleton.Instance.SetArmedWeapon(weapon);
    }
}
