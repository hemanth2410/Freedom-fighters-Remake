using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory System/New Inventory Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] GameObject m_inventoryItemPrefab;
    [SerializeField] InventoryItemType m_itemType;
    [SerializeField] string m_inventoryItemName;
    [SerializeField][TextArea(2, 5)] string m_inventoryItemDescription;

    public GameObject InventoryItemPrefab { get { return m_inventoryItemPrefab; } }
    public InventoryItemType ItemType { get {  return m_itemType; } }
    public string InventoryItemName { get {  return m_inventoryItemName; } }
    public string InventoryItemDescription { get { return m_inventoryItemDescription; } }
}
