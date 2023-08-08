using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Nela {
    public class AssetDeprecationMarkerSettingsDrawer : SettingsProvider {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() {
            return new AssetDeprecationMarkerSettingsDrawer();
        }

        private List<Object> _searchList;

        private AssetDeprecation.DeprecationType _currentDeprecationTypeDisplayed = AssetDeprecation.DeprecationType.None;
        private static Vector2 _scrollPosition;
        private static int _selectedIndex = -1;

        public AssetDeprecationMarkerSettingsDrawer()
            : base("Project/Packages/Asset Deprecation Marker", SettingsScope.Project) {
        }

        public override void OnGUI(string searchContext) {
            var so = AssetDeprecationMarkerSettings.GetSerializedObject();
            so.Update();

            EditorGUI.indentLevel++;
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 200;
            EditorGUILayout.PropertyField(so.FindProperty("_enable"));
            EditorGUILayout.PropertyField(so.FindProperty("_enableNestedDeprecation"));
            EditorGUILayout.PropertyField(so.FindProperty("_deprecatedColor"));
            EditorGUILayout.PropertyField(so.FindProperty("_obsoleteColor"));
            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUI.indentLevel--;

            if (GUI.changed) {
                so.ApplyModifiedProperties();
                AssetDeprecationMarkerSettings.OnSettingsChanged();
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            using (new GUILayout.HorizontalScope()) {
                if (GUILayout.Button(_currentDeprecationTypeDisplayed != AssetDeprecation.DeprecationType.Deprecated ? "List All Deprecated" : "Refresh", EditorStyles.miniButton)) {
                    _searchList = AssetDatabase.FindAssets($"l:{AssetDeprecation.DeprecatedLabel}").Select(guid => {
                        return AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));
                    }).ToList();
                }

                if (GUILayout.Button(_currentDeprecationTypeDisplayed != AssetDeprecation.DeprecationType.Obsolete ? "List All Obsolete" : "Refresh", EditorStyles.miniButton)) {
                    _searchList = AssetDatabase.FindAssets($"l:{AssetDeprecation.ObsoleteLabel}").Select(guid => {
                        return AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));
                    }).ToList();
                }
                GUILayout.FlexibleSpace();
            }

            if (_searchList != null) {
                GUILayout.BeginScrollView(_scrollPosition, GUI.skin.box, GUILayout.MinHeight(160));
                _selectedIndex = ListView.Draw(_searchList, _selectedIndex, o => EditorGUIUtility.ObjectContent(o, o.GetType()));
                GUILayout.EndScrollView();

                if (_selectedIndex != -1) {
                    if (Event.current.type == EventType.MouseDown) {
                        if (Event.current.clickCount == 2) {
                            EditorGUIUtility.PingObject(_searchList[_selectedIndex]);
                            Event.current.Use();
                        }
                    }
                }
            }
        }
    }
}