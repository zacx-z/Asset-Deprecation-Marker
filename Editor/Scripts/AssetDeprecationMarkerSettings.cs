using System;
using UnityEditor;
using UnityEngine;

namespace Nela {
    [FilePath("ProjectSettings/Packages/AssetDeprecationMarkerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AssetDeprecationMarkerSettings : ScriptableSingleton<AssetDeprecationMarkerSettings> {
        public static bool Enabled => instance._enable;
        public static bool EnableNestedDeprecation => instance._enableNestedDeprecation;

        [SerializeField]
        [InspectorName("Enable")]
        private bool _enable = true;

        [SerializeField]
        [Tooltip("Determines whether deprecation markers are displayed on files under a deprecated folder.")]
        private bool _enableNestedDeprecation = false;

        private void OnEnable() {
            hideFlags &= ~HideFlags.NotEditable;
        }

        public static SerializedObject GetSerializedObject() {
            return new SerializedObject(instance);
        }

        public static void OnSettingsChanged() {
            MarkerDrawer.SetEnabled(Enabled);
            EditorApplication.RepaintHierarchyWindow();
            EditorApplication.RepaintProjectWindow();
        }
    }
}