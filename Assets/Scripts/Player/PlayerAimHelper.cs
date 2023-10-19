using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimHelper : MonoBehaviour
{
    [SerializeField] SharedBoolVariable m_aimToggleVariable;
    StarterAssetsInputs inputs;
    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        m_aimToggleVariable.SetValue(inputs.Aim);
    }
}
