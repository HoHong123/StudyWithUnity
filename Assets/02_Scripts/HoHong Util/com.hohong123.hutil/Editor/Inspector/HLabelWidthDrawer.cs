#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * [HLabelWidth] ���� ��ο��Դϴ�.
 * - �ܵ����� �پ��� �� �� ���� �ٲ㼭 �׸��ϴ�.
 * - �ٸ� Ŀ���� ��ο��� �Բ� ���� ���� ����, �Ʒ��� HLabelWidthScope / HDisplayLabelWidthUtil ���۵� �����մϴ�.
 * - HHideLabel�� �Բ� ���� ���� ���� ���¿��� ���븸 �׸���, �� �� ������ ���õ˴ϴ�.
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
            // HHideLabel�� ������ ���� �����, �� �� ������ �ǹ̰� �����Ƿ� �״�� ���븸 �׸���.
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
