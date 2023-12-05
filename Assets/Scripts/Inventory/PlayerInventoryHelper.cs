using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHelper : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_InventorySharedBool;
    StarterAssetsInputs inputs;
    private void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        m_InventorySharedBool.SetValue(inputs.Inventory);
    }
}
