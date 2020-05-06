using System;
using UnityEditor;

namespace Valax321.PlayerController.Editor
{
    internal static class PlayerControllerEdUtility
    {
        public static bool DrawFoldoutCategory(string categoryName, string prefsPath)
        {
            bool state = EditorPrefs.GetBool(prefsPath, true);
            state = EditorGUILayout.BeginFoldoutHeaderGroup(state, categoryName);
            EditorPrefs.SetBool(prefsPath, state);
            return state;
        }

        public static void DrawFoldoutCategory(string categoryName, string prefsPath, Action openFunc)
        {
            if (DrawFoldoutCategory(categoryName, prefsPath))
            {
                openFunc();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}