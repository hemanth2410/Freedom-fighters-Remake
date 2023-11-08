using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectHelper : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_SharedBool;
    [SerializeField] SharedVector3Variable m_SharedVector3;
    [SerializeField] Transform m_TargetTransform;
    [SerializeField] float m_ResetTimer;
    [SerializeField] bool m_BeingRaycasted;
    float resetTimer;
    Camera mainCamera;
    GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        targetObject = m_TargetTransform.gameObject;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        m_BeingRaycasted = m_SharedBool.Value;
        if(m_BeingRaycasted)
        {
            resetTimer = m_ResetTimer;
        }
        if (resetTimer <= 0.0f && !m_BeingRaycasted)
        {
            m_BeingRaycasted = false;
            m_SharedBool.SetValue(false);
            targetObject.SetActive(false);
        }
        if (resetTimer > 0.0f)
        {
            resetTimer -= Time.deltaTime;
            targetObject.SetActive(true);                
        }
        if(m_SharedVector3.Value != null)
        {
            m_TargetTransform.position = mainCamera.WorldToScreenPoint(m_SharedVector3.Value);
        }
    }
}
