#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HMin / HMax�� ó���ϴ� ��ο��Դϴ�.
 * - ��Ƽ-������Ʈ ����.
 * - �Ӽ��� HMin/HMax �� �� ������ [min..max] ������ ����.
 * - ������(int, float, double) �� Vector2/3/4 ������Ʈ�� Ŭ����.
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
            // ���� �ʵ忡 �޸� HMin/HMax�� ��� ����
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

            // ����ڰ� ���� �����ߵ� �ƴϵ�, �׻� ���� ������ ����(�ؽ�Ʈ ����Ʈ �Ŀ��� ����ȭ)
            _ClampProperty(property, minAttribute, maxAttribute);

            // �ؽ�Ʈ �Է� �߿��� ��� ����ǵ��� SerializedObject �ݿ�
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
                    double value = property.doubleValue; // float/double �� �� Ŀ��
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
            // �ʿ� �� Int �迭 ���͵� ���� ����:
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
                // �������� �ƴ� Ÿ���� ����
                // Ȯ�� ����
                break;
            }
        }
    }
}
#endif
