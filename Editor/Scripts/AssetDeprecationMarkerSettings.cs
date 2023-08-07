using System;
using UnityEditor;
using UnityEngine;

namespace Nela {
    [FilePath("ProjectSettings/Packages/AssetDeprecationMarkerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AssetDeprecationMarkerSettings : ScriptableSingleton<AssetDeprecationMarkerSettings> {
        public static bool Enabled => instance._enabled;

        [SerializeField]
        private bool _enabled = true;

        private void OnEnable() {
            hideFlags &= ~HideFlags.NotEditable;
        }

        public static SerializedObject GetSerializedObject() {
            return new SerializedObject(instance);
        }

        public static void OnSettingsChanged() {
            MarkerDrawer.SetEnabled(Enabled);
        }
    }
}