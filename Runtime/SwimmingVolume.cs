using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valax321.PlayerController
{
    /// <summary>
    /// Defines a <see cref="Collider"/> as a swimmable volume for the Player Controller system.
    /// </summary>
    [AddComponentMenu("Player Controller/Swimming Volume")]
    [DisallowMultipleComponent]
    public class SwimmingVolume : MonoBehaviour, IComparable<SwimmingVolume>
    {
        [SerializeField] private int m_depth = 0;
        [SerializeField] private float m_swimSpeed = 4;
        [SerializeField] private float m_swimAcceleration = 6;
        [SerializeField] private float m_swimDeceleration = 10;

        public int depth => m_depth;
        public float swimSpeed => m_swimSpeed;
        public float acceleration => m_swimAcceleration;
        public float deceleration => m_swimDeceleration;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            m_swimSpeed = Mathf.Max(m_swimSpeed, 0);
            m_swimAcceleration = Mathf.Max(m_swimAcceleration, 0);
            m_swimDeceleration = Mathf.Max(m_swimDeceleration, 0);
        }
#endif

        /// <summary>
        /// Compares volume's depth property
        /// </summary>
        /// <param name="other">Other voume to compare to</param>
        public int CompareTo(SwimmingVolume other)
        {
            return m_depth.CompareTo(other.m_depth);
        }
    }
}
