using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Nela {
    [InitializeOnLoad]
    public static class MarkerDrawer {
        static MarkerDrawer() {
            if (AssetDeprecationMarkerSettings.Enabled)
                SetEnabled(true);
        }

        private static bool _enabled = false;

        public static void SetEnabled(bool enabled) {
            if (enabled == _enabled) return;
            if (enabled) {
                EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGUI;
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            } else {
                EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemOnGUI;
                EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;
            }
            _enabled = enabled;
            EditorApplication.RepaintHierarchyWindow();
            EditorApplication.RepaintProjectWindow();
        }

        private static void OnProjectWindowItemOnGUI(string guid, Rect selectionRect) {
            if (IsAssetDeprecated(new GUID(guid))) {
                var rect = GetProjectItemNamePosition(selectionRect, Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
                var strikeThroughRect = new Rect(rect.x, rect.y + rect.height / 2 - 1, rect.width, 2);
                EditorGUI.DrawRect(strikeThroughRect, MarkerStyles.StrikethroughColor);
            }
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceId, Rect selectionRect) {
            var go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go == null) return;
            if (PrefabUtility.IsPartOfPrefabInstance(go)) {
                var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                if (IsAssetDeprecated(AssetDatabase.GUIDFromAssetPath(prefabPath))) {
                    var rect = GetHierarchyItemNamePosition(selectionRect, go);
                    var strikeThroughRect = new Rect(rect.x, rect.y + rect.height / 2 - 1, rect.width, 2);
                    EditorGUI.DrawRect(strikeThroughRect, MarkerStyles.PrefabStrikethroughColor);
                }
            }
        }

        public static bool IsAssetDeprecated(GUID guid) {
            var labels = AssetDatabase.GetLabels(guid);
            return labels.Contains("Deprecated") || labels.Contains("Obsolete");
        }

        private static Rect GetProjectItemNamePosition(Rect itemRect, string name) {
            var labelWidth = MarkerStyles.FileLabelStyle.CalcSize(new GUIContent(name)).x;

            float labelHeight = 12;
            if (itemRect.width > itemRect.height) {
                itemRect.xMin += itemRect.height + 5;
                labelHeight = 14;
            }

            labelWidth = Mathf.Min(itemRect.width, labelWidth);

            var rc = new Rect(itemRect.x, itemRect.yMax - labelHeight, itemRect.width, labelHeight);

            if (itemRect.width < itemRect.height) {
                rc.x = rc.x + rc.width / 2 - labelWidth / 2;
            }
            rc.width = labelWidth;

            return rc;
        }

        private static Rect GetHierarchyItemNamePosition(Rect itemRect, GameObject go) {
            var label = EditorGUIUtility.ObjectContent(go, typeof(GameObject));
            label.image = null;
            var labelWidth = MarkerStyles.FileLabelStyle.CalcSize(label).x;

            itemRect.x += itemRect.height + 1;
            itemRect.width = labelWidth;

            return itemRect;
        }
    }
}