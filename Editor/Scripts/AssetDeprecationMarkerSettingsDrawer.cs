using UnityEditor;
using UnityEngine;

namespace Nela {
    public static class AssetDeprecationMarkerSettingsDrawer {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() {
            return new SettingsProvider("Project/Packages/Asset Deprecation Marker", SettingsScope.Project)
            {
                guiHandler = OnGUI
            };
        }

        private static void OnGUI(string searchContext) {
            var so = AssetDeprecationMarkerSettings.GetSerializedObject();
            so.Update();

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(so.FindProperty("_enable"));
            EditorGUILayout.PropertyField(so.FindProperty("_enableNestedDeprecation"));
            EditorGUI.indentLevel--;

            if (GUI.changed) {
                so.ApplyModifiedProperties();
                AssetDeprecationMarkerSettings.OnSettingsChanged();
            }
        }
    }
}