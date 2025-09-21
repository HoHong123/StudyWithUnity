#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    static class HTitleDrawUtil {
        private static GUIStyle title, subtitle;


        private static void _EnsureStyles(bool bold = true) {
            title ??= new GUIStyle(EditorStyles.label);
            subtitle ??= new GUIStyle(EditorStyles.miniLabel);

            title.fontSize = 13;
            title.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            title.alignment = TextAnchor.MiddleLeft;

            subtitle.fontSize = 10;
            subtitle.fontStyle = FontStyle.Italic;
            subtitle.alignment = TextAnchor.MiddleLeft;
            subtitle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        }


        public static void DrawHeader(HTitleAttribute attr, float width) {
            _EnsureStyles(attr.Bold);

            var align = attr.Alignment == HTitleAttribute.Align.Center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
            title.alignment = align;
            subtitle.alignment = align;

            GUILayout.Space(attr.SpaceBefore);
            GUILayout.Label(attr.Title, title);
            if (!string.IsNullOrEmpty(attr.Subtitle)) {
                GUILayout.Space(-2f);
                GUILayout.Label(attr.Subtitle, subtitle);
            }
            if (attr.HorizontalLine) {
                GUILayout.Space(2f);
                var rect = GUILayoutUtility.GetRect(width, 1f);
                EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), new Color(0.3f, 0.3f, 0.3f));
                GUILayout.Space(3f);
            }
            GUILayout.Space(attr.SpaceAfter);
        }

        public static HTitleAttribute[] GetTypeTitles(UnityEngine.Object target)
            => target
            ? (HTitleAttribute[])Attribute.GetCustomAttributes(target.GetType(), typeof(HTitleAttribute), true)
            : Array.Empty<HTitleAttribute>();
    }

    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    [CanEditMultipleObjects]
    public sealed class HTitleMonoEditor : Editor {
        HTitleAttribute[] titles;

        void OnEnable() => titles = HTitleDrawUtil.GetTypeTitles(target);

        public override void OnInspectorGUI() {
            if (titles != null && titles.Length > 0) {
                float width = EditorGUIUtility.currentViewWidth - 40f;
                foreach (var title in titles) {
                    HTitleDrawUtil.DrawHeader(title, width);
                }
            }
            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
    [CanEditMultipleObjects]
    public sealed class HTitleSOEditor : Editor {
        HTitleAttribute[] titles;

        void OnEnable() => titles = HTitleDrawUtil.GetTypeTitles(target);

        public override void OnInspectorGUI() {
            if (titles != null && titles.Length > 0) {
                float width = EditorGUIUtility.currentViewWidth - 40f;
                foreach (var title in titles) {
                    HTitleDrawUtil.DrawHeader(title, width);
                }
            }
            base.OnInspectorGUI();
        }
    }
}
#endif
