#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    /// <summary>
    /// [HMinMaxSlider(limitMin, limitMax, showFields)]
    /// - Vector2/Vector2Int: MinMax 슬라이더
    /// - float/double/int  : 단일 슬라이더
    /// ※ 변수 참조(동적 한계) 없음 — 상수만.
    /// </summary>
    public sealed class HMinMaxSliderModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, UnityEngine.Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var attr = field.GetCustomAttribute<HUtil.Inspector.HMinMaxSliderAttribute>(inherit: true);
            if (attr == null) return false;

            float min = attr.limitMin;
            float max = attr.limitMax;
            if (min > max) (min, max) = (max, min); 

            switch (serProp.propertyType) {
            case SerializedPropertyType.Vector2:
            case SerializedPropertyType.Vector2Int:
                _DrawMinMaxRange(serProp, min, max, attr.showFields);
                break;
            case SerializedPropertyType.Float:
                if (field.FieldType == typeof(double)) {
                    _DrawDouble(serProp, min, max);
                }
                else {
                    _DrawFloat(serProp, min, max);
                }
                break;
            case SerializedPropertyType.Integer:
                _DrawInt(serProp, Mathf.FloorToInt(min), Mathf.CeilToInt(max));
                break;
            default:
                EditorGUILayout.PropertyField(serProp, true);
                break;
            }

            return true;
        }

        private void _DrawMinMaxRange(SerializedProperty prop, float limitMin, float limitMax, bool showFields) {
            Rect pos = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight);
            var label = new GUIContent(prop.displayName);
            Rect content = EditorGUI.PrefixLabel(pos, label);

            float fieldW = showFields ? 52f : 0f;
            float sp = showFields ? 4f : 0f;

            Rect left = new Rect(content.x, content.y, fieldW, content.height);
            Rect slider = new Rect(content.x + fieldW + sp, content.y, content.width - (fieldW + sp) * 2f, content.height);
            Rect right = new Rect(slider.xMax + sp, content.y, fieldW, content.height);

            float vmin, vmax;
            bool isInt = prop.propertyType == SerializedPropertyType.Vector2Int;
            if (isInt) {
                var v = prop.vector2IntValue;
                vmin = v.x;
                vmax = v.y;
            }
            else {
                var v = prop.vector2Value;
                vmin = v.x;
                vmax = v.y;
            }

            if (showFields) vmin = EditorGUI.FloatField(left, vmin);
            EditorGUI.MinMaxSlider(slider, ref vmin, ref vmax, limitMin, limitMax);
            if (showFields) vmax = EditorGUI.FloatField(right, vmax);

            if (vmin > vmax) (vmin, vmax) = (vmax, vmin);
            vmin = Mathf.Clamp(vmin, limitMin, limitMax);
            vmax = Mathf.Clamp(vmax, limitMin, limitMax);

            if (isInt) {
                prop.vector2IntValue = new Vector2Int(Mathf.RoundToInt(vmin), Mathf.RoundToInt(vmax));
            }
            else {
                prop.vector2Value = new Vector2(vmin, vmax);
            }
        }

        private void _DrawFloat(SerializedProperty serProp, float min, float max) {
            float next = EditorGUILayout.Slider(serProp.displayName, serProp.floatValue, min, max);
            serProp.floatValue = Mathf.Clamp(next, min, max);
        }

        private void _DrawDouble(SerializedProperty serProp, float min, float max) {
            double curr = serProp.doubleValue;
            float next = EditorGUILayout.Slider(serProp.displayName, (float)curr, min, max);
            serProp.doubleValue = Mathf.Clamp(next, min, max);
        }

        private void _DrawInt(SerializedProperty serProp, int min, int max) {
            int next = EditorGUILayout.IntSlider(serProp.displayName, serProp.intValue, min, max);
            serProp.intValue = Mathf.Clamp(next, min, max);
        }
    }
}
#endif
