using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valax321.PlayerController
{
    /// <summary>
    /// Base class for input handlers on <see cref="PlayerMovement"/>
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class PlayerInputSource : MonoBehaviour
    {
        public abstract float GetForwardAxis();
        public abstract float GetRightAxis();

        public abstract float GetLookXAxis();
        public abstract float GetLookYAxis();

        public abstract bool GetJumpPressed();
        public abstract bool GetSprintHeld();
        public abstract bool GetCrouchHeld();

        public abstract bool GetToggleNoclip();

        public abstract float GetLookSensitivity();
    }
}