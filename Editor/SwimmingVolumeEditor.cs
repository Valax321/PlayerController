using System;
using UnityEditor;

namespace Valax321.PlayerController.Editor
{
    [CustomEditor(typeof(SwimmingVolume))]
    public class SwimmingVolumeEditor : UnityEditor.Editor
    {
        private const string PrefsPath = "Valax321:SwimmingVolumeEditor:";
        
        private SerializedProperty m_depth;
        private SerializedProperty m_swimSpeed;
        private SerializedProperty m_swimAcceleration;
        private SerializedProperty m_swimDeceleration;
        
        private void OnEnable()
        {
            m_depth = serializedObject.FindProperty("m_depth");
            m_swimSpeed = serializedObject.FindProperty("m_swimSpeed");
            m_swimAcceleration = serializedObject.FindProperty("m_swimAcceleration");
            m_swimDeceleration = serializedObject.FindProperty("m_swimDeceleration");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(m_depth);
            
            EditorGUILayout.Separator();
            
            PlayerControllerEdUtility.DrawFoldoutCategory("Movement", PrefsPath + "Movement", () =>
            {
                EditorGUILayout.PropertyField(m_swimSpeed);
                EditorGUILayout.PropertyField(m_swimAcceleration);
                EditorGUILayout.PropertyField(m_swimDeceleration);
            });

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}