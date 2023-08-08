using System.Linq;
using UnityEditor;

namespace Nela {
    public class DeprecationMenu {
        public const string DeprecateMenuPath = "Assets/Deprecation Marker/Deprecate";
        public const string ObsoleteMenuPath = "Assets/Deprecation Marker/Obsolete";

        [MenuItem(DeprecateMenuPath)]
        public static void DeprecateAsset() {
            var wasDeprecated = Menu.GetChecked(DeprecateMenuPath);

            foreach (var obj in Selection.objects) {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out var guid, out long _)) {
                    var labels = AssetDatabase.GetLabels(new GUID(guid));
                    var labelsNoDeprecation = labels.Where(s => s != AssetDeprecation.DeprecatedLabel && s != AssetDeprecation.ObsoleteLabel);
                    if (wasDeprecated) {
                        labels = labelsNoDeprecation.ToArray();
                    } else {
                        labels = labelsNoDeprecation.Append(AssetDeprecation.DeprecatedLabel).ToArray();
                    }

                    AssetDatabase.SetLabels(obj, labels);
                }
            }
        }

        [MenuItem(DeprecateMenuPath, true)]
        public static bool ValidDeprecateAsset() {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var valid = !string.IsNullOrEmpty(path);
            if (valid) {
                AssetDeprecation.IsAssetDeprecated(AssetDatabase.GUIDFromAssetPath(path), out var type);
                Menu.SetChecked(DeprecateMenuPath, type == AssetDeprecation.DeprecationType.Deprecated);
                Menu.SetChecked(ObsoleteMenuPath, type == AssetDeprecation.DeprecationType.Obsolete);
            }

            return valid;
        }

        [MenuItem(ObsoleteMenuPath)]
        public static void ObsoleteAsset() {
            var wasObsolete = Menu.GetChecked(ObsoleteMenuPath);

            foreach (var obj in Selection.objects) {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out var guid, out long _)) {
                    var labels = AssetDatabase.GetLabels(new GUID(guid));
                    var labelsNoDeprecation = labels.Where(s => s != AssetDeprecation.DeprecatedLabel && s != AssetDeprecation.ObsoleteLabel);
                    if (wasObsolete) {
                        labels = labelsNoDeprecation.ToArray();
                    } else {
                        labels = labelsNoDeprecation.Append(AssetDeprecation.ObsoleteLabel).ToArray();
                    }

                    AssetDatabase.SetLabels(obj, labels);
                }
            }
        }

        [MenuItem(ObsoleteMenuPath, true)]
        public static bool ValidObsoleteAsset() {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return !string.IsNullOrEmpty(path);
        }
    }
}