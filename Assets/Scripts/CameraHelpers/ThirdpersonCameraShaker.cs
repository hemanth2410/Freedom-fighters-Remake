using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdpersonCameraShaker : MonoBehaviour
{
    [SerializeField] SharedFloatVariable m_cameraEffectSharedFloat;
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cinemachineBasicMultiChannelPerlin == null)
            return;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = m_cameraEffectSharedFloat.Value;
    }
}
