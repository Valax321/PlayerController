using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valax321.PlayerController
{
    /// <summary>
    /// The best Unity character controller ever!
    /// </summary>
    [AddComponentMenu("Player Controller/Player Movement")]
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class PlayerMovement : MonoBehaviour
    {
        #region Inspector Properties
        
        [SerializeField] private float m_walkSpeed = 5;
        [SerializeField] private float m_runSpeed = 8;
        [SerializeField] private float m_crouchSpeed = 3;
        [SerializeField] private float m_groundAcceleration = 25;
        [SerializeField] private float m_groundDeceleration = 40;
        [SerializeField] private float m_airAcceleration = 3;
        [SerializeField] private float m_jumpForce = 4;
        
        [SerializeField] private float m_radius = 0.4f;
        [SerializeField] private float m_standingHeight = 1.8f;
        [SerializeField] private float m_crouchingHeight = 1;
        [SerializeField] private float m_crouchBlendSpeed = 4;
        
        [SerializeField] private Transform m_cameraRoot = default;
        [SerializeField] private float m_standingCameraHeight = 0.5f;
        [SerializeField] private float m_crouchedCameraHeight = 0.1f;
        [SerializeField] private float m_maxLookAngle = 90;
        [SerializeField] private float m_minLookAngle = -90;

        #endregion

        #region Private Fields

        private Transform m_transform;
        private CharacterController m_controller;
        private bool m_wasPreviouslyGrounded = false;
        private Vector3 m_velocity;
        private Collider[] m_crouchCheckColliders;
        private bool m_noClip;
        private SortedSet<SwimmingVolume> m_swimVolumes = new SortedSet<SwimmingVolume>();
        private PlayerInputSource m_inputSource;
        private float m_camPitch;

        #endregion

        #region Public Getters

        public Vector3 velocity => m_velocity;
        public bool isGrounded => m_wasPreviouslyGrounded;

        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// Ensures the <see cref="CharacterController"/> is configured properly.
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            var cc = GetComponent<CharacterController>();
            if (cc)
            {
                cc.minMoveDistance = 0;
                cc.height = m_standingHeight;
                cc.radius = m_radius;
                //cc.stepOffset = 0; // Not yet
            }

            if (m_cameraRoot)
            {
                m_cameraRoot.localPosition = new Vector3(0, m_standingCameraHeight, 0);
            }

            m_walkSpeed = Mathf.Max(m_walkSpeed, 0);
            m_runSpeed = Mathf.Max(m_runSpeed, 0);
            m_crouchSpeed = Mathf.Max(m_crouchSpeed, 0);
        }
#endif

        private void Awake()
        {
            m_controller = GetComponent<CharacterController>();
            m_transform = transform;

            m_controller.radius = m_radius;
            m_crouchCheckColliders = new Collider[1];

            m_inputSource = GetComponent<PlayerInputSource>();
            if (!m_inputSource)
            {
                Debug.LogError("No PlayerInputSource found for PlayerMovement", this);
            }

            if (m_cameraRoot)
            {
                m_camPitch = m_cameraRoot.localEulerAngles.x;
            }
        }

        private void Update()
        {
            if (m_inputSource)
            {
                UpdateCamera();
                UpdateMovement();
            }
        }

        /// <summary>
        /// Updates the player rotation based on look input.
        /// </summary>
        private void UpdateCamera()
        {
            if (!m_cameraRoot)
                return;

            var sensitivity = m_inputSource.GetLookSensitivity();

            var mx = m_inputSource.GetLookXAxis() * sensitivity;
            var my = m_inputSource.GetLookYAxis() * sensitivity * -1;

            m_transform.Rotate(m_transform.up, mx);

            var camLocals = m_cameraRoot.localEulerAngles;
            m_camPitch = Mathf.Clamp(m_camPitch + my, m_minLookAngle, m_maxLookAngle);
            camLocals.x = m_camPitch;
            m_cameraRoot.localEulerAngles = camLocals;
        }

        /// <summary>
        /// Updates the player position and velocity through the <see cref="CharacterController"/>
        /// </summary>
        private void UpdateMovement()
        {
            if (m_inputSource.GetToggleNoclip())
            {
                m_noClip = !m_noClip;
            }

            var input = Vector2.ClampMagnitude(
                new Vector2(m_inputSource.GetRightAxis(), m_inputSource.GetForwardAxis()), 1);

            var wishdir = (m_transform.forward * input.y) + (m_transform.right * input.x);

            var wishspeed = m_walkSpeed;

            bool isCrouching = false;

            if (m_inputSource.GetSprintHeld())
            {
                wishspeed = m_runSpeed;
            }

            if (m_inputSource.GetCrouchHeld() || m_swimVolumes.Count > 0)
            {
                wishspeed = m_crouchSpeed;
                isCrouching = true;
            }

            //Interpolate height
            float crouchInterpDist = m_controller.height;

            // Check if we can stand back up
            bool canStand = true;
            if (!Mathf.Approximately(m_controller.height, m_standingHeight) && !isCrouching)
            {
                var projectedFloorPoint =
                    (m_transform.up * -1) *
                    (m_controller.height /
                     2); //Where we expect the floor to be (the controller will push up out of the floor automatically)
                var projectedCapsuleHeight = projectedFloorPoint + (m_transform.up * m_standingHeight);
                canStand = !Physics.CheckCapsule(projectedFloorPoint, projectedCapsuleHeight, m_controller.radius,
                    LayerMask.GetMask("Character"), QueryTriggerInteraction.Ignore);
                isCrouching = !canStand;
            }

            if (canStand)
            {
                m_controller.height = Mathf.MoveTowards(m_controller.height,
                    isCrouching ? m_crouchingHeight : m_standingHeight, m_crouchBlendSpeed * Time.deltaTime);
            }

            crouchInterpDist = crouchInterpDist - m_controller.height; //Calculate offset

            if (m_cameraRoot)
            {
                var lp = m_cameraRoot.localPosition;
                float l = Mathf.Lerp(m_crouchedCameraHeight, m_standingCameraHeight,
                    Mathf.InverseLerp(m_crouchingHeight, m_standingHeight, m_controller.height));
                lp.y = l;
                m_cameraRoot.localPosition = lp;
            }

            bool badSlope = false;
            Vector3 slopeDir = Physics.gravity.normalized;

            if (m_wasPreviouslyGrounded && GetGround(out RaycastHit hit))
            {
                //Floor slope calculations                
                Vector3 slopeForward = Vector3.Cross(hit.normal, -m_transform.right);
                Vector3 slopeRight = Vector3.Cross(hit.normal, slopeForward);

                wishdir = (slopeForward * input.y) + (slopeRight * input.x); //Recalculate the move direction
                slopeDir = Vector3.Cross(Vector3.Cross(hit.normal, Physics.gravity.normalized), hit.normal);

                if (Vector3.Angle(hit.normal, m_transform.up) > m_controller.slopeLimit)
                    badSlope = true;
            }

            var gravity = Physics.gravity;

            if (m_noClip) //Noclip mode
            {
                wishdir = (m_cameraRoot.transform.forward * input.y) + (m_cameraRoot.transform.right * input.x);
                gravity = Vector3.zero;
            }

            if (m_swimVolumes.Count <= 0 && m_wasPreviouslyGrounded && !badSlope) //Was on the ground
            {
                if (wishdir.magnitude > 0)
                {
                    m_velocity = Vector3.MoveTowards(m_velocity, wishdir * wishspeed,
                        m_groundAcceleration * Time.deltaTime);
                }
                else
                {
                    m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, m_groundDeceleration * Time.deltaTime);
                }

                m_wasPreviouslyGrounded = m_controller
                    .Move((m_velocity * Time.deltaTime) + (m_transform.up * -m_controller.stepOffset))
                    .HasFlag(CollisionFlags.Below);

                if (isCrouching && Mathf.Abs(crouchInterpDist) > 0)
                {
                    m_controller.Move(m_transform.up * -Mathf.Abs(crouchInterpDist));
                }

                if (m_inputSource.GetJumpPressed() && m_wasPreviouslyGrounded)
                {
                    m_wasPreviouslyGrounded = false;
                    m_velocity += m_transform.up * m_jumpForce;
                    SendMessage("Jumped", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    m_velocity = m_controller.velocity;
                }
            }
            else if (m_swimVolumes.Count > 0)
            {
                var volume = m_swimVolumes.Last();
                
                wishdir = (m_cameraRoot.transform.forward * input.y) + (m_cameraRoot.transform.right * input.x);
                if (wishdir.magnitude > 0)
                {
                    m_velocity = Vector3.MoveTowards(m_velocity, wishdir * volume.swimSpeed,
                        volume.acceleration * Time.deltaTime);
                }
                else
                {
                    m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, volume.deceleration * Time.deltaTime);
                }

                m_controller.Move(m_velocity * Time.deltaTime);
                m_velocity = m_controller.velocity;
            }
            else // In the air
            {
                if (m_noClip)
                {
                    m_velocity = (wishdir * (m_airAcceleration * 5));
                    transform.position += m_velocity * Time.deltaTime;
                }
                else
                {
                    m_velocity += (wishdir * m_airAcceleration) * Time.deltaTime;
                    m_velocity += gravity.magnitude * slopeDir * Time.deltaTime;
                    var mv = m_controller.Move(m_velocity * Time.deltaTime);
                    m_wasPreviouslyGrounded = mv.HasFlag(CollisionFlags.Below);
                }

                m_velocity = m_controller.velocity;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var swim = other.GetComponentInParent<SwimmingVolume>();
            if (swim && !m_swimVolumes.Contains(swim))
            {
                m_swimVolumes.Add(swim);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var swim = other.GetComponentInParent<SwimmingVolume>();
            if (swim && m_swimVolumes.Contains(swim))
            {
                m_swimVolumes.Remove(swim);
            }
        }

        private bool GetGround(out RaycastHit hit)
        {
            return Physics.SphereCast(m_transform.position, m_controller.radius, m_transform.up * -1, out hit,
                m_controller.height / 2 +
                m_controller.stepOffset /*, LayerMask.GetMask("Character"), QueryTriggerInteraction.Ignore*/);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);
            bool inverse = false;
            var tmin = min;
            var tangle = angle;
            if (min > 180)
            {
                inverse = !inverse;
                tmin -= 180;
            }

            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }

            var result = !inverse ? tangle > tmin : tangle < tmin;
            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;
            var tmax = max;
            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }

            if (max > 180)
            {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if (!result)
                angle = max;
            return angle;
        }
    }
}