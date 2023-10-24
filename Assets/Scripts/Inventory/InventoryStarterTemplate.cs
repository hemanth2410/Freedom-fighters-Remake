using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Starter Template", menuName = "Inventory System/New Starter Template")]
public class InventoryStarterTemplate : ScriptableObject
{
    [SerializeField] List<InventoryItem> m_InventoryItems;
    public List<InventoryItem> InventoryItems { get { return m_InventoryItems; } }
}
