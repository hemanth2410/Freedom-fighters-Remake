using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField][Tooltip("Override this value if Root bone is of different scale")] float m_weaponScaleMultiplier = 1.0f;
    [SerializeField] Transform m_handBone;
    [SerializeField] float m_coolDown;
    [SerializeField] Transform m_DropTransform;
    PlayerInventory playerInventory;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    bool isArmed;
    bool cooldown;
    Weapon armedWeapon;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        WeaponsSingleton.Instance.WeaponPicked += Instance_WeaponPicked;
    }

    private void Instance_WeaponPicked(InventoryItem obj)
    {
        if (isArmed)
        {
            armedWeapon.gameObject.SetActive(false);
            isArmed = false;
            if (armedWeapon.WeaponData.ItemType == obj.ItemType)
            {
                // same item probably destroy and perform cleanup
                dropWeapon(armedWeapon.WeaponData);
                Destroy(armedWeapon.gameObject);
            }
        }
        // Now change the armed weapon
        var _k = Instantiate(obj.InventoryItemPrefab, m_handBone);
        _k.transform.localPosition = obj.PositionOffset;
        _k.transform.localRotation = Quaternion.Euler(obj.RotationOffset);
        _k.transform.localScale *= m_weaponScaleMultiplier;
        _k.GetComponent<Collider>().isTrigger = true;
        _k.GetComponent<Rigidbody>().isKinematic = true;
        _k.gameObject.layer = LayerMask.NameToLayer("Melee");
        armedWeapon = _k.GetComponent<Weapon>();
        isArmed = true;
    }

    private void ChangeActiveWeapon(InventoryItem  obj)
    {
        if(isArmed)
        {
            armedWeapon.gameObject.SetActive(false);
            isArmed = false;
            if(armedWeapon.WeaponData.ItemType == obj.ItemType)
            {
                // same item probably destroy and perform cleanup
                dropWeapon(armedWeapon.WeaponData);
                Destroy(armedWeapon.gameObject);
            }
        }
        // Now change the armed weapon
        var _k = Instantiate(obj.InventoryItemPrefab, m_handBone);
        _k.transform.localPosition = obj.PositionOffset;
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
}
