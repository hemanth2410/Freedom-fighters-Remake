using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedBoolArrayEnabler : MonoBehaviour
{
    [SerializeField] List<SharedBoolVariable> m_SharedBools;
    [SerializeField] bool m_Negate;
    [SerializeField] GameObject m_ObjectToToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ObjectToToggle.SetActive(m_SharedBools.All(x => x.Value) && !m_Negate);
    }
}
