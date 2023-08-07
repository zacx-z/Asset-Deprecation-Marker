using System.Linq;
using UnityEditor;

namespace Nela {
    public class DeprecationMenu {
        public const string DeprecateMenuPath = "Assets/Deprecation Marker/Deprecate";

        [MenuItem(DeprecateMenuPath)]
        public static void DeprecateAsset() {
            var wasDeprecated = Menu.GetChecked(DeprecateMenuPath);

            foreach (var obj in Selection.objects) {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out var guid, out long _)) {
                    var labels = AssetDatabase.GetLabels(new GUID(guid));
                    if (wasDeprecated) {
                        labels = labels.Where(s => s != "Deprecated" && s != "Obsolete").ToArray();
                    } else {
                        labels = labels.Append("Deprecated").ToArray();
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
                Menu.SetChecked(DeprecateMenuPath, MarkerDrawer.IsAssetDeprecated(AssetDatabase.GUIDFromAssetPath(path)));
            }

            return valid;
        }
    }
}