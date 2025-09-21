#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HMin / HMax를 처리하는 드로워입니다.
 * - 멀티-오브젝트 편집.
 * - 속성에 HMin/HMax 둘 다 붙으면 [min..max] 범위로 강제.
 * - 숫자형(int, float, double) 및 Vector2/3/4 컴포넌트별 클램프.
 * =========================================================
 */
using System.Linq;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    /// <summary>
    /// Drawer that handles HMin/HMax
    /// </summary>
    [CustomPropertyDrawer(typeof(HMinAttribute), useForChildren: true)]
    [CustomPropertyDrawer(typeof(HMaxAttribute), useForChildren: true)]
    public class HRangeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // 같은 필드에 달린 HMin/HMax를 모두 수집
            var minAttribute = fieldInfo
                .GetCustomAttributes(typeof(HMinAttribute), inherit: true)
                .Cast<HMinAttribute>()
                .FirstOrDefault();

            var maxAttribute = fieldInfo
                .GetCustomAttributes(typeof(HMaxAttribute), inherit: true)
                .Cast<HMaxAttribute>()
                .FirstOrDefault();

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, includeChildren: true);
            bool isDrawChanged = EditorGUI.EndChangeCheck();

            // 사용자가 값을 수정했든 아니든, 항상 제한 범위로 스냅(텍스트 에디트 후에도 정상화)
            _ClampProperty(property, minAttribute, maxAttribute);

            // 텍스트 입력 중에도 즉시 적용되도록 SerializedObject 반영
            if (isDrawChanged)
                property.serializedObject.ApplyModifiedProperties();
        }

        private static void _ClampProperty(SerializedProperty property, HMinAttribute minAttribute, HMaxAttribute maxAttribute) {
            bool hasMin = minAttribute != null;
            bool hasMax = maxAttribute != null;

            if (!hasMin && !hasMax) return;

            double minValue = hasMin ? minAttribute.MinValue : double.NegativeInfinity;
            double maxValue = hasMax ? maxAttribute.MaxValue : double.PositiveInfinity;

            double ClampScalar(double value) {
                if (value < minValue) value = minValue;
                if (value > maxValue) value = maxValue;
                return value;
            }

            switch (property.propertyType) {
            case SerializedPropertyType.Integer: {
                    double value = property.intValue;
                    property.intValue = (int)ClampScalar(value);
                    break;
                }
            case SerializedPropertyType.Float: {
                    double value = property.doubleValue; // float/double 둘 다 커버
                    property.doubleValue = ClampScalar(value);
                    break;
                }
            case SerializedPropertyType.Vector2: {
                    Vector2 vector = property.vector2Value;
                    vector.x = (float)ClampScalar(vector.x);
                    vector.y = (float)ClampScalar(vector.y);
                    property.vector2Value = vector;
                    break;
                }
            case SerializedPropertyType.Vector3: {
                    Vector3 vector = property.vector3Value;
                    vector.x = (float)ClampScalar(vector.x);
                    vector.y = (float)ClampScalar(vector.y);
                    vector.z = (float)ClampScalar(vector.z);
                    property.vector3Value = vector;
                    break;
                }
            case SerializedPropertyType.Vector4: {
                    Vector4 vector = property.vector4Value;
                    vector.x = (float)ClampScalar(vector.x);
                    vector.y = (float)ClampScalar(vector.y);
                    vector.z = (float)ClampScalar(vector.z);
                    vector.w = (float)ClampScalar(vector.w);
                    property.vector4Value = vector;
                    break;
                }
            // 필요 시 Int 계열 벡터도 지원 가능:
            case SerializedPropertyType.Vector2Int: {
                    Vector2Int vector = property.vector2IntValue;
                    vector.x = (int)ClampScalar(vector.x);
                    vector.y = (int)ClampScalar(vector.y);
                    property.vector2IntValue = vector;
                    break;
                }
            case SerializedPropertyType.Vector3Int: {
                    Vector3Int vector = property.vector3IntValue;
                    vector.x = (int)ClampScalar(vector.x);
                    vector.y = (int)ClampScalar(vector.y);
                    vector.z = (int)ClampScalar(vector.z);
                    property.vector3IntValue = vector;
                    break;
                }
            default:
                // 숫자형이 아닌 타입은 무시
                // 확장 가능
                break;
            }
        }
    }
}
#endif
