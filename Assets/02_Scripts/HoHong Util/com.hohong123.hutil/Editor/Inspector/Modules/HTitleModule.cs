#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public sealed class HTitleModule : HBaseModule {
        static GUIStyle titleStyle;
        static GUIStyle _TitleStyle {
            get {
                if (titleStyle == null) {
                    titleStyle = new GUIStyle(EditorStyles.boldLabel) {
                        fontSize = 12,
                        alignment = TextAnchor.MiddleLeft
                    };
                }
                return titleStyle;
            }
        }


        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var attr = field.GetCustomAttribute<HUtil.Inspector.HTitleAttribute>(inherit: true);
            if (attr == null) return false;

            var title = string.IsNullOrEmpty(attr.Title) ? serProp.displayName : attr.Title;

            EditorGUILayout.Space(6f);
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox)) {
                GUILayout.Label(title, _TitleStyle);
            }
            EditorGUILayout.Space(2f);

            EditorGUILayout.PropertyField(serProp, true);
            return true;
        }
    }
}
#endif
