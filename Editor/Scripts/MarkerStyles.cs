using UnityEditor;
using UnityEngine;

namespace Nela {
    public static class MarkerStyles {
        public static Color StrikethroughColor = new Color(0.9f, 0.9f, 0.9f);
        public static Color PrefabStrikethroughColor = new Color(0.488f, 0.684f, 0.965f);

        public static GUIStyle FileLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 12
        };

        public static Color HighlightedColor = new Color(0.238f, 0.328f, 0.527f);
    }
}