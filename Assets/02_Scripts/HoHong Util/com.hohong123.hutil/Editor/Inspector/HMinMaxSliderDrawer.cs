#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    [CustomPropertyDrawer(typeof(HMinMaxSliderAttribute), useForChildren: true)]
    public class HMinMaxSliderDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.Vector2 &&
                property.propertyType != SerializedPropertyType.Vector2Int) {
                EditorGUI.HelpBox(position, "[HMinMaxSlider] supports Vector2 / Vector2Int only.", MessageType.Warning);
                return;
            }

            HMinMaxSliderAttribute attr = (HMinMaxSliderAttribute)attribute;

            using (HDisplayLabelWidthUtil.UseIfAny(fieldInfo)) {
                label = HDisplayLabelUtil.ResolveLabel(fieldInfo, label);
                Rect contentRect = EditorGUI.PrefixLabel(position, label);

                float fieldWidth = attr.showFields ? 48f : 0f;
                float spacing = attr.showFields ? 4f : 0f;
                Rect leftField = new Rect(contentRect.x, contentRect.y, fieldWidth, contentRect.height);
                Rect sliderRect = new Rect(contentRect.x + fieldWidth + spacing, contentRect.y,
                                            contentRect.width - (fieldWidth + spacing) * 2f, contentRect.height);
                Rect rightField = new Rect(sliderRect.xMax + spacing, contentRect.y, fieldWidth, contentRect.height);

                float minValue, maxValue;
                if (property.propertyType == SerializedPropertyType.Vector2) {
                    Vector2 range = property.vector2Value;
                    minValue = range.x;
                    maxValue = range.y;
                }
                else {
                    Vector2Int range = property.vector2IntValue;
                    minValue = range.x;
                    maxValue = range.y;
                }

                if (attr.showFields) {
                    minValue = EditorGUI.FloatField(leftField, minValue);
                }

                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, attr.limitMin, attr.limitMax);

                if (attr.showFields) {
                    maxValue = EditorGUI.FloatField(rightField, maxValue);
                }

                // 정렬 및 한계값 클램프
                if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);
                minValue = Mathf.Clamp(minValue, attr.limitMin, attr.limitMax);
                maxValue = Mathf.Clamp(maxValue, attr.limitMin, attr.limitMax);

                if (property.propertyType == SerializedPropertyType.Vector2) {
                    property.vector2Value = new Vector2(minValue, maxValue);
                }
                else {
                    property.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minValue), Mathf.RoundToInt(maxValue));
                }
            }
        }
    }
}
#endif
