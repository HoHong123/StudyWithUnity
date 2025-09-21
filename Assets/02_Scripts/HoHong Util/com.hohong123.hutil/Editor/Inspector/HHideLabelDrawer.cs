#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    /* =========================================================
     * @Jason - PKH
     * �ܵ����� �پ��� �� ���� ���ܼ� �׷��ִ� ���� ��ο��Դϴ�.
     * �ٸ� ��ο��� �Բ� ���̸� Unity ���� ��Ģ�� �� ��ο��� ȣ����� ���� �� �ֽ��ϴ�.
     * =========================================================
     */
    [CustomPropertyDrawer(typeof(HHideLabelAttribute), useForChildren: true)]
    public class HHideLabelDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // �� ����
            EditorGUI.PropertyField(position, property, GUIContent.none, includeChildren: true);
        }
    }

    /* =========================================================
     * @Jason - PKH
     * �ٸ� Ŀ���� ��ο�(HDisplayDrawer/HRangeDrawer/HOnValueChangedDrawer)������
     * HHideLabel�� �����ϵ��� ���� ������ ���� Ŭ�����Դϴ�.
     * =========================================================
     */
    public static class HDisplayLabelUtil {
        public static GUIContent ResolveLabel(FieldInfo fieldInfo, GUIContent originalLabel) {
            if (fieldInfo == null) return originalLabel ?? GUIContent.none;

            bool hasHideLabel =
                fieldInfo.GetCustomAttributes(typeof(HHideLabelAttribute), inherit: true)
                         .Cast<HHideLabelAttribute>()
                         .Any();

            return hasHideLabel ? GUIContent.none : (originalLabel ?? GUIContent.none);
        }
    }
}
#endif
