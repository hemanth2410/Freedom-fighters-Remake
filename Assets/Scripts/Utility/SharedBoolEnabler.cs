using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedBoolEnabler : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_SharedBool;
    [SerializeField] bool m_Negate;
    [SerializeField] GameObject m_ObjectToToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ObjectToToggle.SetActive(m_SharedBool.Value && !m_Negate);
    }
}
