#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 인스펙터에서 값이 바뀌면 지정 메서드를 호출하는 속성입니다.
 * - 값 변경 시 SerializedObject 적용 후 대상 오브젝트들의 메서드 호출.
 * - 매개변수 0개 또는 1개 메서드 지원(1개면 새 값을 전달, 가능한 경우 형변환 시도).
 * - 멀티 오브젝트 편집 지원: 모든 선택 대상 각각에 대해 호출.
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
            // 속성에 달린 모든 HOnValueChangedAttribute 수집(여러 개 가능)
            var onValueChangedAttributes = fieldInfo
                .GetCustomAttributes(typeof(HOnValueChangedAttribute), inherit: true)
                .Cast<HOnValueChangedAttribute>()
                .ToArray();

            bool hasAttributes = onValueChangedAttributes != null && onValueChangedAttributes.Length > 0;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, includeChildren: true);

            // 값이 바뀐 경우에만 호출
            if (EditorGUI.EndChangeCheck()) {
                // 실제 값을 적용한 뒤에 콜백을 호출해야 최신 값이 들어간다.
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

            // 1) 새 값 1개를 받는 메서드 우선 탐색(형 호환 또는 변환 가능 시)
            MethodInfo[] candidateMethods = type.GetMethods(flags)
                                                .Where(m => m.Name == methodName)
                                                .ToArray();

            // 1.1) 단일 매개변수 메서드 우선
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

            // 1.2) 매개변수 없는 메서드 차선
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

            // 1.3) 마지막 시도: 단일 매개변수 메서드가 있지만 변환이 실패했던 경우,
            //     원시 새 값을 억지로 전달(유니티 커스텀 타입 등의 경우 개발자가 직접 맞추도록).
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
                // 널 허용 참조형 또는 Nullable<T>면 통과
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
                    // 문자열 -> enum 이름
                    if (value is string enumName && Enum.IsDefined(requiredType, enumName))
                        return Enum.Parse(requiredType, enumName, ignoreCase: true);
                }

                // 숫자형 등 기본 변환
                return Convert.ChangeType(value, requiredType);
            }
            catch {
                return converterFailureSentinel;
            }
        }

        private static object _GetBoxedValue(SerializedProperty property) {
            // 가능한 범용적으로 박싱. 필요한 타입만 우선 지원.
            switch (property.propertyType) {
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.doubleValue; // double로 받음(정밀도)
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
                return null; // 그 외 특수 타입은 개발자가 직접 메서드 시그니처를 맞추면 됨
            }
        }
    }
}
#endif
