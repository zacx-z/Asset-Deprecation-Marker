using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Nela {
    public class ListView {
        public static int Draw<T>(IList<T> list, int selectedIndex, Func<T, GUIContent> labelGetter) {
            int prevElemId = -1;
            for (int i = 0; i < list.Count; i++) {
                var id = GUIUtility.GetControlID(FocusType.Keyboard);
                var elemRect = GUILayoutUtility.GetRect(10, 1000, 16, 16);

                if (i == selectedIndex) GUIUtility.keyboardControl = id;

                if (GUIUtility.keyboardControl == prevElemId) {
                    if (Event.current.type == EventType.KeyDown) {
                        if (Event.current.keyCode == KeyCode.DownArrow) {
                            GUIUtility.hotControl = GUIUtility.keyboardControl = id;
                            Event.current.Use();
                        }
                    }
                }

                if (GUIUtility.keyboardControl == id) {
                    EditorGUI.DrawRect(elemRect, MarkerStyles.HighlightedColor);
                    selectedIndex = i;

                    if (Event.current.type == EventType.KeyDown) {
                        if (prevElemId != -1 && Event.current.keyCode == KeyCode.UpArrow) {
                            GUIUtility.hotControl = GUIUtility.keyboardControl = prevElemId;
                            Event.current.Use();
                        }
                    }
                }

                GUI.Label(elemRect, labelGetter(list[i]), EditorStyles.miniLabel);

                if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)
                    && Event.current.clickCount == 1 && elemRect.Contains(Event.current.mousePosition)) {
                    GUIUtility.hotControl = GUIUtility.keyboardControl = id;
                    selectedIndex = i;
                    Event.current.Use();
                }

                prevElemId = id;
            }

            if (selectedIndex >= list.Count) selectedIndex = -1;

            return selectedIndex;
        }
    }
}