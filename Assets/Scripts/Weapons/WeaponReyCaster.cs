using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReyCaster : MonoBehaviour
{
    [SerializeField] LayerMask m_WeaponsLayer;
    [SerializeField] string m_HighlightLayer;
    [SerializeField] float m_RaycastRange;
    [SerializeField] SharedVector3Variable m_SharedVector3;
    [SerializeField] SharedBoolVariable m_SharedBool;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, m_RaycastRange, m_WeaponsLayer))
        {
            // change weapon layer and display UI to pickup.
            hit.collider.GetComponent<Weapon>().UpdateWeaponLayer(m_HighlightLayer);
            m_SharedBool.SetValue(true);
            m_SharedVector3.SetValue(hit.collider.transform.position);
        }
        else
        {
            m_SharedBool.SetValue(false);
        }
    }
}
