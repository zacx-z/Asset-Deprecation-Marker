using System;
using UnityEditor;
using UnityEngine;

namespace Nela {
    [FilePath("ProjectSettings/Packages/AssetDeprecationMarkerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AssetDeprecationMarkerSettings : ScriptableSingleton<AssetDeprecationMarkerSettings> {
        public static bool Enabled => instance._enable;
        public static bool EnableNestedDeprecation => instance._enableNestedDeprecation;
        public static Color DeprecatedColor => instance._deprecatedColor;
        public static Color ObsoleteColor => instance._obsoleteColor;

        [SerializeField]
        [InspectorName("Enable")]
        private bool _enable = true;

        [SerializeField]
        [Tooltip("Determines whether deprecation markers are displayed on files under a deprecated folder.")]
        private bool _enableNestedDeprecation = false;

        [Header("Strikethrough Color")]
        [SerializeField]
        private Color _deprecatedColor = Color.white;
        [SerializeField]
        private Color _obsoleteColor = new Color(1, 0.5f, 0.5f);

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