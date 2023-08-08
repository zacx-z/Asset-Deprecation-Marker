using System.IO;
using System.Linq;
using UnityEditor;

namespace Nela {
    public static class AssetDeprecation {
        public enum DeprecationType {
            None,
            Deprecated,
            Obsolete
        }
        
        public const string DeprecatedLabel = "Deprecated";
        public const string ObsoleteLabel = "Obsolete";

        public static bool IsAssetDeprecated(GUID guid, out AssetDeprecation.DeprecationType deprecationType) {
            var flag = AssetDeprecation.IsAssetDeprecatedSelf(guid);
            if (flag != AssetDeprecation.DeprecationType.None) {
                deprecationType = flag;
                return true;
            }

            deprecationType = AssetDeprecation.DeprecationType.None;

            if (AssetDeprecationMarkerSettings.EnableNestedDeprecation) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path)) return false;

                path = Path.GetDirectoryName(path);
                while (!string.IsNullOrEmpty(path)) {
                    flag = AssetDeprecation.IsAssetDeprecatedSelf(AssetDatabase.GUIDFromAssetPath(path));
                    if (flag != AssetDeprecation.DeprecationType.None) {
                        deprecationType = flag;
                        return true;
                    }
                    path = Path.GetDirectoryName(path);
                }
            }

            return false;
        }

        private static AssetDeprecation.DeprecationType IsAssetDeprecatedSelf(GUID guid) {
            var labels = AssetDatabase.GetLabels(guid);
            if (labels.Contains("Deprecated")) {
                return AssetDeprecation.DeprecationType.Deprecated;
            }

            if (labels.Contains("Obsolete")) {
                return AssetDeprecation.DeprecationType.Obsolete;
            }

            return AssetDeprecation.DeprecationType.None;
        }
    }
}