#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    /* =========================================================
     * @Jason - PKH
     * 단독으로 붙었을 때 라벨을 숨겨서 그려주는 전용 드로워입니다.
     * 다른 드로워와 함께 쓰이면 Unity 선택 규칙상 이 드로워가 호출되지 않을 수 있습니다.
     * =========================================================
     */
    [CustomPropertyDrawer(typeof(HHideLabelAttribute), useForChildren: true)]
    public class HHideLabelDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // 라벨 제거
            EditorGUI.PropertyField(position, property, GUIContent.none, includeChildren: true);
        }
    }

    /* =========================================================
     * @Jason - PKH
     * 다른 커스텀 드로워(HDisplayDrawer/HRangeDrawer/HOnValueChangedDrawer)에서도
     * HHideLabel을 존중하도록 재사용 가능한 헬퍼 클래스입니다.
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
