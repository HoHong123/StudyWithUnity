#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * [HLabelWidth] 전용 드로워입니다.
 * - 단독으로 붙었을 때 라벨 폭을 바꿔서 그립니다.
 * - 다른 커스텀 드로워와 함께 쓰일 때를 위해, 아래의 HLabelWidthScope / HDisplayLabelWidthUtil 헬퍼도 제공합니다.
 * - HHideLabel과 함께 쓰면 라벨을 숨긴 상태에서 내용만 그리되, 라벨 폭 변경은 무시됩니다.
 * =========================================================
 */
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    [CustomPropertyDrawer(typeof(HLabelWidthAttribute), useForChildren: true)]
    public class HLabelWidthDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // HHideLabel이 있으면 라벨을 숨기고, 라벨 폭 설정은 의미가 없으므로 그대로 내용만 그린다.
            label = HDisplayLabelUtil.ResolveLabel(fieldInfo, label);
            if (ReferenceEquals(label, GUIContent.none)) {
                EditorGUI.PropertyField(position, property, GUIContent.none, includeChildren: true);
                return;
            }

            var widthAttribute = (HLabelWidthAttribute)attribute;
            using (new HLabelWidthScope(widthAttribute.Width)) {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
            }
        }
    }

    public struct HLabelWidthScope : System.IDisposable {
        private readonly float previousWidth;
        private readonly bool applied;

        public HLabelWidthScope(float desiredWidth) {
            previousWidth = EditorGUIUtility.labelWidth;
            if (desiredWidth > 0f) {
                EditorGUIUtility.labelWidth = desiredWidth;
                applied = true;
            }
            else {
                applied = false;
            }
        }

        public void Dispose() {
            if (applied) EditorGUIUtility.labelWidth = previousWidth;
        }
    }

    public static class HDisplayLabelWidthUtil {
        private sealed class NoopScope : System.IDisposable {
            public static readonly NoopScope Instance = new NoopScope();
            public void Dispose() { }
        }

        public static System.IDisposable UseIfAny(FieldInfo fieldInfo) {
            if (fieldInfo == null) return NoopScope.Instance;

            var attr = fieldInfo
                .GetCustomAttributes(typeof(HLabelWidthAttribute), inherit: true)
                .Cast<HLabelWidthAttribute>()
                .FirstOrDefault();

            return (attr != null && attr.Width > 0f)
                ? new HLabelWidthScope(attr.Width)
                : NoopScope.Instance;
        }
    }
}
#endif
