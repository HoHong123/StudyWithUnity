#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public sealed class HLimitModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var minA = field.GetCustomAttribute<HUtil.Inspector.HMinAttribute>(inherit: true);
            var maxA = field.GetCustomAttribute<HUtil.Inspector.HMaxAttribute>(inherit: true);
            if (minA == null && maxA == null) return false;

            EditorGUILayout.PropertyField(serProp, true);

            double min = minA?.MinValue ?? double.NegativeInfinity;
            double max = maxA?.MaxValue ?? double.PositiveInfinity;

            switch (serProp.propertyType) {
            case SerializedPropertyType.Integer: {
                    serProp.intValue = (int)Mathf.Clamp(serProp.intValue, (float)min, (float)max);
                    break;
                }
            case SerializedPropertyType.Float: {
                    if (field.FieldType == typeof(double)) {
                        double value = serProp.doubleValue;
                        if (value < min) value = min;
                        if (value > max) value = max;
                        serProp.doubleValue = value;
                    }
                    else {
                        serProp.floatValue = Mathf.Clamp(serProp.floatValue, (float)min, (float)max);
                    }
                    break;
                }
            default:
                // Vector, 기타 타입 확장 가능.
                break;
            }

            return true;
        }
    }
}
#endif
