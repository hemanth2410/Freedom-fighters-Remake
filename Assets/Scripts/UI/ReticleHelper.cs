using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleHelper : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_aimToggleVariable;
    Image reticleImage;
    // Start is called before the first frame update
    void Start()
    {
        reticleImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        reticleImage.enabled = m_aimToggleVariable.Value;
    }
}
