using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventoryStarterTemplate m_StarterTemplate;
    InventoryUI inventoryUI;
    List<InventoryItem> playerInventoryItems = new List<InventoryItem>();
    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        if(m_StarterTemplate != null)
        {
            // here we will have to load them into a shared variable to avoid cyclic dependancies.
        }
        inventoryUI.ConstructInventoryUI();
    }
    // Update is called once per frame
    void Update()
    {
        // Handle Inpyt logic and pickup logic
        // Just like reticle we use a shared variable to display
        // The UI of Inventory. A shared boolean.
    }
}
