#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public sealed class HOnValueChangedModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var attr = field.GetCustomAttribute<HUtil.Inspector.HOnValueChangedAttribute>(inherit: true);
            if (attr == null) return false;

            // Prev snapshot
            object oldVal = _ReadManagedValue(serProp, field, target);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serProp, true);
            if (EditorGUI.EndChangeCheck()) {
                so.ApplyModifiedProperties();
                object newVal = _ReadManagedValue(serProp, field, target);
                _InvokeCallbacks(target, attr.MethodName, field, oldVal, newVal);
            }
            return true;
        }


        private void _InvokeCallbacks(object target, string methodName, FieldInfo fi, object oldVal, object newVal) {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = target.GetType();
            var method = type.GetMethod(methodName, flags);
            if (method == null) return;

            var ps = method.GetParameters();
            try {
                if (ps.Length == 0) {
                    method.Invoke(target, null);
                }
                else if (ps.Length == 1) {
                    method.Invoke(target, new[] { newVal });
                }
                else if (ps.Length == 2) {
                    method.Invoke(target, new[] { oldVal, newVal });
                }
                else {
                    method.Invoke(target, null);
                }
            }
            catch {
            }
        }

        private object _ReadManagedValue(SerializedProperty serProp, FieldInfo field, object target) {
            try {
                return field.GetValue(target);
            }
            catch {
            }

            switch (serProp.propertyType) {
            case SerializedPropertyType.Integer:
                return serProp.intValue;
            case SerializedPropertyType.Float:
                return (field.FieldType == typeof(double)) ? serProp.doubleValue : (object)serProp.floatValue;
            case SerializedPropertyType.Boolean:
                return serProp.boolValue;
            case SerializedPropertyType.String:
                return serProp.stringValue;
            case SerializedPropertyType.Vector2:
                return serProp.vector2Value;
            case SerializedPropertyType.Vector2Int:
                return serProp.vector2IntValue;
            case SerializedPropertyType.Color:
                return serProp.colorValue;
            case SerializedPropertyType.ObjectReference:
                return serProp.objectReferenceValue;
            default:
                return null;
            }
        }
    }
}
#endif
