using UnityEngine;

namespace Valax321.PlayerController
{
    /// <summary>
    /// Input source using Unity input system
    /// </summary>
    [AddComponentMenu("Player Controller/Input Sources/Unity Input Source")]
    [DisallowMultipleComponent]
    public class UnityInputSource : PlayerInputSource
    {
        [Header("Camera")] 
        [SerializeField] private string m_lookXAxis =  "Mouse X";
        [SerializeField] private string m_lookYAxis = "Mouse Y";
        [SerializeField] private float m_sensitivity = 1.0f;
    
        [Header("Movement")] 
        [SerializeField] private string m_moveForward = "Vertical";
        [SerializeField] private string m_moveRight = "Horizontal";
        [SerializeField] private string m_jump = "Jump";
        [SerializeField] private string m_sprint = "Fire3";
        [SerializeField] private string m_crouch = "Fire1";

        [Header("Debug")] 
        [SerializeField] private bool m_allowNoclipInBuild = false;
        [SerializeField] private string m_noclip = "Submit";

        /// <summary>
        /// Camera look sensitivity multiplier
        /// </summary>
        public float sensitivity
        {
            get => m_sensitivity;
            set => m_sensitivity = value;
        }
        
        public override float GetForwardAxis()
        {
            return Input.GetAxis(m_moveForward);
        }

        public override float GetRightAxis()
        {
            return Input.GetAxis(m_moveRight);
        }

        public override float GetLookXAxis()
        {
            return Input.GetAxis(m_lookXAxis);
        }

        public override float GetLookYAxis()
        {
            return Input.GetAxis(m_lookYAxis);
        }

        public override bool GetJumpPressed()
        {
            return Input.GetButtonDown(m_jump);
        }

        public override bool GetSprintHeld()
        {
            return Input.GetButton(m_sprint);
        }

        public override bool GetCrouchHeld()
        {
            return Input.GetButton(m_crouch);
        }

        public override bool GetToggleNoclip()
        {
            return m_allowNoclipInBuild ? 
                Input.GetButtonDown(m_noclip) : 
                Application.isEditor && Input.GetButtonDown(m_noclip);
        }

        public override float GetLookSensitivity()
        {
            return m_sensitivity;
        }
    }
}