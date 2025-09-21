#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HReadOnlyAttribute�� ��ξ� ��ũ��Ʈ�Դϴ�.
 * + ��� Ÿ���� �����ϱ� ���� includeChildren = true�� ������
 * + �⺻/Ŀ���� ������Ƽ ���̸� �״�� ����
 * =========================================================
 */
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    [CustomPropertyDrawer(typeof(HReadOnlyAttribute), /*useForChildren:*/ true)]
    public sealed class HReadOnlyDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var attr = (HReadOnlyAttribute)attribute;
            // OnlyInPlayMode=true�� �÷��� ���� ���� ���, �ƴϸ� �׻� ���
            bool disable = !attr.OnlyInPlayMode || (attr.OnlyInPlayMode && Application.isPlaying);

            // children���� �����ؼ� �׸��� �迭/����ü/Ŭ���� ������ �ڵ� ����
            using (new EditorGUI.DisabledScope(disable)) {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
            }
        }
    }
}
#endif
