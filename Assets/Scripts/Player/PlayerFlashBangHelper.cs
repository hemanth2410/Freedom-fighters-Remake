using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashBangHelper : MonoBehaviour, IFlashbangObsorver
{
    [SerializeField] SharedBoolVariable m_goBlindBool;
    [SerializeField] float m_flashTimer;
    float resetTimer;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(resetTimer > 0.0f)
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer <= 0.0f)
            {
                resetTimer = 0.0f;
                m_goBlindBool.SetValue(false);
            }
        }
    }
    public void ReactToFlashbang(Vector3 position)
    {
        

        // check if flash is within the camera frustum
        bool _blind = true;
        var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        foreach (var plane in planes)
        {
            if(plane.GetDistanceToPoint(position) < 0)
            {
                _blind = false; return;
            }
        }
        // check if player or camera is seeing the flash bang
        // check if there are any obstacles in the way.
        RaycastHit _hit;
        Vector3 _direction = position - mainCamera.transform.position;
        if(Physics.Raycast(mainCamera.transform.position, _direction, out _hit))
        {
            if(_hit.collider.GetComponent<FlashbangThrowable>()) 
            {
                // player has seen the flash
                m_goBlindBool.SetValue(true);
            }
        }
        resetTimer = m_flashTimer;
    }
}
