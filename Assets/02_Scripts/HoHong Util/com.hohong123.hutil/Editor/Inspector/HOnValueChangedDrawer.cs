#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �ν����Ϳ��� ���� �ٲ�� ���� �޼��带 ȣ���ϴ� �Ӽ��Դϴ�.
 * - �� ���� �� SerializedObject ���� �� ��� ������Ʈ���� �޼��� ȣ��.
 * - �Ű����� 0�� �Ǵ� 1�� �޼��� ����(1���� �� ���� ����, ������ ��� ����ȯ �õ�).
 * - ��Ƽ ������Ʈ ���� ����: ��� ���� ��� ������ ���� ȣ��.
 * =========================================================
 */
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    [CustomPropertyDrawer(typeof(HOnValueChangedAttribute), useForChildren: true)]
    public class HOnValueChangedDrawer : PropertyDrawer {
        private static readonly object converterFailureSentinel = new object();


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // �Ӽ��� �޸� ��� HOnValueChangedAttribute ����(���� �� ����)
            var onValueChangedAttributes = fieldInfo
                .GetCustomAttributes(typeof(HOnValueChangedAttribute), inherit: true)
                .Cast<HOnValueChangedAttribute>()
                .ToArray();

            bool hasAttributes = onValueChangedAttributes != null && onValueChangedAttributes.Length > 0;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, includeChildren: true);

            // ���� �ٲ� ��쿡�� ȣ��
            if (EditorGUI.EndChangeCheck()) {
                // ���� ���� ������ �ڿ� �ݹ��� ȣ���ؾ� �ֽ� ���� ����.
                property.serializedObject.ApplyModifiedProperties();

                if (hasAttributes) {
                    object newValueBoxed = _GetBoxedValue(property);

                    UnityEngine.Object[] selectedTargets = property.serializedObject.targetObjects;
                    foreach (UnityEngine.Object selectedObject in selectedTargets) {
                        _InvokeAllCallbacks(selectedObject, onValueChangedAttributes, newValueBoxed);
                    }
                }
            }
        }


        private static void _InvokeAllCallbacks(object targetObject, HOnValueChangedAttribute[] attributes, object newValueBoxed) {
            if (targetObject == null || attributes == null) return;
            foreach (var attr in attributes) {
                _InvokeSingleCallback(targetObject, attr.MethodName, newValueBoxed);
            }
        }

        private static void _InvokeSingleCallback(object targetObject, string methodName, object newValueBoxed) {
            if (targetObject == null || string.IsNullOrEmpty(methodName))
                return;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type type = targetObject.GetType();

            // 1) �� �� 1���� �޴� �޼��� �켱 Ž��(�� ȣȯ �Ǵ� ��ȯ ���� ��)
            MethodInfo[] candidateMethods = type.GetMethods(flags)
                                                .Where(m => m.Name == methodName)
                                                .ToArray();

            // 1.1) ���� �Ű����� �޼��� �켱
            foreach (MethodInfo method in candidateMethods) {
                var parameters = method.GetParameters();
                if (parameters.Length == 1) {
                    object argument = _ConvertArgument(parameters[0].ParameterType, newValueBoxed);
                    if (argument != converterFailureSentinel) {
                        try { method.Invoke(targetObject, new[] { argument }); }
                        catch (Exception exception) {
                            Debug.LogException(exception);
                        }
                        return;
                    }
                }
            }

            // 1.2) �Ű����� ���� �޼��� ����
            foreach (MethodInfo method in candidateMethods) {
                var parameters = method.GetParameters();
                if (parameters.Length == 0) {
                    try { method.Invoke(targetObject, null); }
                    catch (Exception exception) {
                        Debug.LogException(exception);
                    }
                    return;
                }
            }

            // 1.3) ������ �õ�: ���� �Ű����� �޼��尡 ������ ��ȯ�� �����ߴ� ���,
            //     ���� �� ���� ������ ����(����Ƽ Ŀ���� Ÿ�� ���� ��� �����ڰ� ���� ���ߵ���).
            foreach (MethodInfo method in candidateMethods) {
                var parameters = method.GetParameters();
                if (parameters.Length == 1) {
                    try { method.Invoke(targetObject, new[] { newValueBoxed }); }
                    catch (Exception exception) {
                        Debug.LogException(exception);
                    }
                    return;
                }
            }
        }

        private static object _ConvertArgument(Type requiredType, object value) {
            if (value == null) {
                // �� ��� ������ �Ǵ� Nullable<T>�� ���
                if (!requiredType.IsValueType || Nullable.GetUnderlyingType(requiredType) != null)
                    return null;
                return converterFailureSentinel;
            }

            Type valueType = value.GetType();
            if (requiredType.IsAssignableFrom(valueType))
                return value;

            try {
                if (requiredType.IsEnum) {
                    // int -> enum
                    if (value is int enumIndex)
                        return Enum.ToObject(requiredType, enumIndex);
                    // ���ڿ� -> enum �̸�
                    if (value is string enumName && Enum.IsDefined(requiredType, enumName))
                        return Enum.Parse(requiredType, enumName, ignoreCase: true);
                }

                // ������ �� �⺻ ��ȯ
                return Convert.ChangeType(value, requiredType);
            }
            catch {
                return converterFailureSentinel;
            }
        }

        private static object _GetBoxedValue(SerializedProperty property) {
            // ������ ���������� �ڽ�. �ʿ��� Ÿ�Ը� �켱 ����.
            switch (property.propertyType) {
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.doubleValue; // double�� ����(���е�)
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.Enum:
                return property.enumValueIndex;
            case SerializedPropertyType.Color:
                return property.colorValue;
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue;
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            case SerializedPropertyType.Vector3:
                return property.vector3Value;
            case SerializedPropertyType.Vector4:
                return property.vector4Value;
            case SerializedPropertyType.Rect:
                return property.rectValue;
            case SerializedPropertyType.Bounds:
                return property.boundsValue;
            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue;
            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue;
            case SerializedPropertyType.RectInt:
                return property.rectIntValue;
            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue;
            case SerializedPropertyType.Quaternion:
                return property.quaternionValue;
            default:
                return null; // �� �� Ư�� Ÿ���� �����ڰ� ���� �޼��� �ñ״�ó�� ���߸� ��
            }
        }
    }
}
#endif
