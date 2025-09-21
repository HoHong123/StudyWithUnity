#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    [CustomPropertyDrawer(typeof(HTitleAttribute))]
    public sealed class HTitleDecoratorDrawer : DecoratorDrawer {
        private static GUIStyle title, subtitle;


        private static void _EnsureStyles(bool bold = true) {
            title ??= new GUIStyle(EditorStyles.label);
            subtitle ??= new GUIStyle(EditorStyles.miniLabel);

            title.fontSize = 12;
            title.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            title.alignment = TextAnchor.MiddleLeft;

            subtitle.fontSize = 10;
            subtitle.fontStyle = FontStyle.Italic;
            subtitle.alignment = TextAnchor.MiddleLeft;
            subtitle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        }


        public override float GetHeight() {
            var attr = (HTitleAttribute)attribute;
            float height = attr.SpaceBefore + EditorGUIUtility.singleLineHeight;
            if (!string.IsNullOrEmpty(attr.Subtitle))
                height += EditorGUIUtility.singleLineHeight - 2f;
            if (attr.HorizontalLine)
                height += 4f;
            height += attr.SpaceAfter;
            return height;
        }

        public override void OnGUI(Rect position) {
            var attr = (HTitleAttribute)attribute;
            _EnsureStyles(attr.Bold);

            float posY = position.y + attr.SpaceBefore;
            float posW = position.width;

            var align = attr.Alignment == HTitleAttribute.Align.Center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
            title.alignment = align;
            subtitle.alignment = align;

            var titleRect = new Rect(position.x, posY, posW, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(titleRect, attr.Title, title);
            posY += EditorGUIUtility.singleLineHeight;

            if (!string.IsNullOrEmpty(attr.Subtitle)) {
                var subRect = new Rect(position.x, posY - 2f, posW, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(subRect, attr.Subtitle, subtitle);
                posY += EditorGUIUtility.singleLineHeight - 2f;
            }

            if (attr.HorizontalLine) {
                posY += 2f;
                var lineRect = new Rect(position.x, posY, posW, 1f);
                EditorGUI.DrawRect(lineRect, new Color(0.3f, 0.3f, 0.3f));
                posY += 3f;
            }
        }
    }
}
#endif
