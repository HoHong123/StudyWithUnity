#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HReadOnlyAttribute용 드로어 스크립트입니다.
 * + 모든 타입을 포괄하기 위해 includeChildren = true로 렌더링
 * + 기본/커스텀 프로퍼티 높이를 그대로 따름
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
            // OnlyInPlayMode=true면 플레이 중일 때만 잠금, 아니면 항상 잠금
            bool disable = !attr.OnlyInPlayMode || (attr.OnlyInPlayMode && Application.isPlaying);

            // children까지 포함해서 그리면 배열/구조체/클래스 전개도 자동 대응
            using (new EditorGUI.DisabledScope(disable)) {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
            }
        }
    }
}
#endif
