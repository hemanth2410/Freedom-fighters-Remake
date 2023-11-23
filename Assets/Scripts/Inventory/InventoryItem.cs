using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory System/New Inventory Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] GameObject m_inventoryItemPrefab;
    [SerializeField] InventoryItemType m_itemType;
    [SerializeField] ShotType m_shotType;
    [SerializeField] HandlingType m_handlingType;
    [SerializeField][Tooltip("We typically want IK for two handed and single handed projectile weapons")] 
    bool m_RequiresIK = false;
    [SerializeField] string m_inventoryItemName;
    [SerializeField][TextArea(2, 5)] string m_inventoryItemDescription;
    [SerializeField] Vector3 m_positionOffset;
    [SerializeField] Vector3 m_rotationOffset;
    public GameObject InventoryItemPrefab { get { return m_inventoryItemPrefab; } }
    public InventoryItemType ItemType { get {  return m_itemType; } }
    public HandlingType HandlingType { get { return m_handlingType; } }
    public ShotType ShotType {  get { return m_shotType; } }
    public bool RequiresIK { get { return m_RequiresIK; } }
    public string InventoryItemName { get {  return m_inventoryItemName; } }
    public string InventoryItemDescription { get { return m_inventoryItemDescription; } }
    public Vector3 PositionOffset { get {  return m_positionOffset; } }
    public Vector3 RotationOffset { get { return m_rotationOffset; } }
}
