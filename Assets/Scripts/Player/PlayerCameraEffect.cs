using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraEffect : MonoBehaviour
{
    [SerializeField] SharedFloatVariable m_cameraEffectSharedFloat;
    [SerializeField] float m_maxAmplitudeGain;
    StarterAssetsInputs inputs;
    private void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }
    // Update is called once per frame
    void Update()
    {
        m_cameraEffectSharedFloat.SetValue(inputs.Sprint ? m_maxAmplitudeGain : 0);
    }
}
