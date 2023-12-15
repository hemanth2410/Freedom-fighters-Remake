using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
#endif

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(StarterAssetsInputs))]
#endif
public class ThirdpersonController : MonoBehaviour
{
    #region Variables
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] float m_moveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    [SerializeField] float m_sprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    [SerializeField] float m_rotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] float m_speedChangeRate = 10.0f;

    [SerializeField] AudioClip m_landingAudioClip;
    [SerializeField] AudioClip[] m_footstepAudioClips;
    [Range(0, 1)][SerializeField] float m_footstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    [SerializeField] float m_jumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] float m_gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] float m_jumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] float m_fallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] bool m_grounded = true;

    [Tooltip("Useful for rough ground")]
    [SerializeField] float m_groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] float m_groundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    [SerializeField] LayerMask m_groundLayers;
    [SerializeField] LayerMask m_aimLayers; 

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] GameObject m_cinemachineCameraTarget;
    [SerializeField] GameObject m_CinemachineSniperTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] float m_topClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] float m_bottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField] float m_cameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    [SerializeField] bool m_lockCameraPosition = false;

    [Tooltip("Sensitivity when normal")]
    [SerializeField] float m_normalSensitivity;

    [Tooltip("Sensitivity when aiming")]
    [SerializeField] float m_aimSensitivity;
    [SerializeField] SharedVector3Variable m_AimPosition;
    [SerializeField] Transform m_aimPositionTransform;

    // cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    // player
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    // animation IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;
    private int animIDCrouch;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif
    private Animator animator;
    private CharacterController controller;
    private StarterAssetsInputs input;
    private GameObject mainCamera;
    private Camera activeCamera;
    private const float threshold = 0.01f;

    private Vector3 targetDirection;
    private bool hasAnimator;
    private bool lockMovement;
    private Vector3 aimPosition;
    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }
    #endregion

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main.gameObject;
            activeCamera = Camera.main;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cinemachineTargetYaw = m_cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        hasAnimator = TryGetComponent(out animator);
        controller = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.LogError("Starter asset package is missing dependancies");
#endif
        // Get animation Hashes
        AssignAnimationIDs();
        jumpTimeoutDelta = m_jumpTimeout;
        fallTimeoutDelta = m_fallTimeout;
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDJump = Animator.StringToHash("Jump");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFreeFall = Animator.StringToHash("Freefall");
        animIDCrouch = Animator.StringToHash("Crouch");
    }

    // Update is called once per frame
    void Update()
    {
        aimRaycast();
        jumpAndGravity();
        groundedCheck();
        move();
    }
    private void LateUpdate()
    {
        cameraRotation();
    }

    void aimRaycast()
    {
        Vector2 _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray _ray = activeCamera.ScreenPointToRay(_screenCenter);
        // A good addition can be if the player is holding a sniper rifle may be increase it to 1000m
        if (Physics.Raycast(_ray, out RaycastHit hit, 1000f, m_aimLayers))
        {
            aimPosition = hit.point;
            m_AimPosition.SetValue(hit.point);
            m_aimPositionTransform.position = hit.point;
        }
        else
        {
            aimPosition = mainCamera.transform.forward * 1000.0f;
            m_AimPosition.SetValue(aimPosition);
            m_aimPositionTransform.position = aimPosition;
        }
    }

    void groundedCheck()
    {
        Vector3 _spherePosition = new Vector3(transform.position.x, transform.position.y - m_groundedOffset, transform.position.z);
        m_grounded = Physics.CheckSphere(_spherePosition, m_groundedRadius, m_groundLayers, QueryTriggerInteraction.Ignore);
        // Now in animator we set a bool to grounded.
        animator.SetBool(animIDGrounded, m_grounded);
        lockMovement = !m_grounded;
    }
    void move()
    {
        float _targetSpeed = input.Sprint && !input.Aim ? m_sprintSpeed : input.Crouch ? m_moveSpeed * 0.5f : m_moveSpeed;
        if (input.Move == Vector2.zero) _targetSpeed = 0.0f;
        float _currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        float _speedOffset = 0.1f;
        float _inputMagnitude = input.AnalogMovement ? input.Move.magnitude : 1.0f;
        if(_currentHorizontalSpeed < _targetSpeed - _speedOffset || _currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            speed = Mathf.Lerp(_currentHorizontalSpeed, _targetSpeed * _inputMagnitude, Time.deltaTime * m_speedChangeRate);
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = _targetSpeed;
        }
        animationBlend = Mathf.Lerp(animationBlend, _targetSpeed, Time.deltaTime * m_speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0.0f;
        Vector3 _inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;
        if (input.Aim)
        {
            Vector3 _worldAimPosition = aimPosition;
            _worldAimPosition.y = transform.position.y;
            Vector3 _aimDirection = (_worldAimPosition - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, _aimDirection, Time.deltaTime * 20.0f);
        }
        if (input.Move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, m_rotationSmoothTime);
            if(!lockMovement && !input.Aim)
                transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
        }
        if (!lockMovement)
        {
            targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        if(hasAnimator)
        {
            animator.SetBool(animIDCrouch, input.Crouch);
            animator.SetFloat(animIDSpeed, speed);
            animator.SetFloat(animIDMotionSpeed, _inputMagnitude);
        }
    }
    void jumpAndGravity()
    {
        if (m_grounded)
        {
            fallTimeoutDelta = m_fallTimeout;
            if(hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }
            if(verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }
            if(input.Jump && jumpTimeoutDelta < 0.0f && !input.Crouch)
            {
                // only add this when player is not armed
                if(WeaponsSingleton.Instance.ArmedWeapon == null ||
                   WeaponsSingleton.Instance.ArmedWeapon.WeaponData == null || 
                   WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ShotConfigration == null || 
                   WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ShotConfigration.HandlingType == HandlingType.SingleHand) 
                {
                    verticalVelocity = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                }
                if(hasAnimator)
                {
                    animator.SetBool(animIDJump, true);
                }
                
            }
            //if(input.Jump && jumpTimeoutDelta < 0.0f && !input.Crouch && WeaponsSingleton.Instance.ArmedWeapon.WeaponData.ShotConfigration.HandlingType != HandlingType.DualHand)
            //{
            //    if (hasAnimator)
            //    {
            //        animator.SetBool(animIDJump, true);
            //    }
            //}
            if(jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = m_jumpHeight;
            if(fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if(hasAnimator)
                {
                    animator.SetBool(animIDFreeFall, true);
                }
            }
            input.Jump = false;
        }

        if(verticalVelocity < terminalVelocity)
        {
            verticalVelocity += m_gravity * Time.deltaTime;
        }
    }

    void cameraRotation()
    {
        if(input.Look.sqrMagnitude >=threshold && !m_lockCameraPosition)
        {
            float _deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _deltaTimeMultiplier *= input.Aim ? m_aimSensitivity : m_normalSensitivity;
            cinemachineTargetYaw += input.Look.x * _deltaTimeMultiplier;
            cinemachineTargetPitch += input.Look.y * _deltaTimeMultiplier;
        }
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, m_bottomClamp, m_topClamp);
        m_cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + m_cameraAngleOverride, cinemachineTargetYaw, 0.0f);
        m_CinemachineSniperTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + m_cameraAngleOverride, cinemachineTargetYaw, 0.0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (m_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - m_groundedOffset, transform.position.z),
            m_groundedRadius);
    }
}
