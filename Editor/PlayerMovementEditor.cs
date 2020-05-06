using System;
using UnityEditor;

namespace Valax321.PlayerController.Editor
{
    [CustomEditor(typeof(PlayerMovement))]
    internal class PlayerMovementEditor : UnityEditor.Editor
    {
        private const string PrefsPath = "Valax321:PlayerMovementEditor:";
        
        private SerializedProperty m_walkSpeed;
        private SerializedProperty m_runSpeed;
        private SerializedProperty m_crouchSpeed;
        private SerializedProperty m_groundAcceleration;
        private SerializedProperty m_groundDeceleration;
        private SerializedProperty m_airAcceleration;
        private SerializedProperty m_jumpForce;

        private SerializedProperty m_radius;
        private SerializedProperty m_standingHeight;
        private SerializedProperty m_crouchingHeight;
        private SerializedProperty m_crouchBlendSpeed;

        private SerializedProperty m_cameraRoot;
        private SerializedProperty m_standingCameraHeight;
        private SerializedProperty m_crouchedCameraHeight;
        private SerializedProperty m_maxLookAngle;
        private SerializedProperty m_minLookAngle;
        
        private void OnEnable()
        {
            m_walkSpeed = serializedObject.FindProperty("m_walkSpeed");
            m_runSpeed = serializedObject.FindProperty("m_runSpeed");
            m_crouchSpeed = serializedObject.FindProperty("m_crouchSpeed");
            m_groundAcceleration = serializedObject.FindProperty("m_groundAcceleration");
            m_groundDeceleration = serializedObject.FindProperty("m_groundDeceleration");
            m_airAcceleration = serializedObject.FindProperty("m_airAcceleration");
            m_jumpForce = serializedObject.FindProperty("m_jumpForce");

            m_radius = serializedObject.FindProperty("m_radius");
            m_standingHeight = serializedObject.FindProperty("m_standingHeight");
            m_crouchingHeight = serializedObject.FindProperty("m_crouchingHeight");
            m_crouchBlendSpeed = serializedObject.FindProperty("m_crouchBlendSpeed");

            m_cameraRoot = serializedObject.FindProperty("m_cameraRoot");
            m_standingCameraHeight = serializedObject.FindProperty("m_standingCameraHeight");
            m_crouchedCameraHeight = serializedObject.FindProperty("m_crouchedCameraHeight");
            m_maxLookAngle = serializedObject.FindProperty("m_maxLookAngle");
            m_minLookAngle = serializedObject.FindProperty("m_minLookAngle");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            if (PlayerControllerEdUtility.DrawFoldoutCategory("Movement", PrefsPath + "Movement"))
            {
                EditorGUILayout.PropertyField(m_walkSpeed);
                EditorGUILayout.PropertyField(m_runSpeed);
                EditorGUILayout.PropertyField(m_crouchSpeed);
                EditorGUILayout.PropertyField(m_groundAcceleration);
                EditorGUILayout.PropertyField(m_groundDeceleration);
                EditorGUILayout.PropertyField(m_airAcceleration);
                EditorGUILayout.PropertyField(m_jumpForce);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Separator();

            if (PlayerControllerEdUtility.DrawFoldoutCategory("Size", PrefsPath + "Size"))
            {
                EditorGUILayout.PropertyField(m_radius);
                EditorGUILayout.PropertyField(m_standingHeight);
                EditorGUILayout.PropertyField(m_crouchingHeight);
                EditorGUILayout.PropertyField(m_crouchBlendSpeed);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Separator();

            if (PlayerControllerEdUtility.DrawFoldoutCategory("Camera", PrefsPath + "Camera"))
            {
                EditorGUILayout.PropertyField(m_cameraRoot);
                EditorGUILayout.PropertyField(m_standingCameraHeight);
                EditorGUILayout.PropertyField(m_crouchedCameraHeight);
                DrawMinMaxField("Look Angles", m_minLookAngle, m_maxLookAngle);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Separator();

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        void DrawMinMaxField(string name, SerializedProperty min, SerializedProperty max)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(name);
            min.floatValue = EditorGUILayout.FloatField(min.floatValue);
            max.floatValue = EditorGUILayout.FloatField(max.floatValue);
            EditorGUILayout.EndHorizontal();
        }
    }
}