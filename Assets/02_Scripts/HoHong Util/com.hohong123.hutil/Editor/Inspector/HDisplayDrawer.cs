#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using HUtil.Inspector;

namespace HEditor.Inspector {
    /// <summary>
    /// 단일 드로워가 HShowIf/HHideIf 모두 처리.
    /// </summary>
    ///  - HHideIf: 같은 필드 내 여러 HHideIf는 OR. 멀티선택 시 모든 대상이 숨김을 원할 때만 숨김.
    ///  - HShowIf: 같은 필드 내 여러 HShowIf는 AND. 멀티선택 시 모든 대상이 전부 만족해야 표시.
    [CustomPropertyDrawer(typeof(HShowIfAttribute), useForChildren: true)]
    [CustomPropertyDrawer(typeof(HHideIfAttribute), useForChildren: true)]
    public class HDisplayDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return _ShouldDrawProperty(property)
                ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true)
                : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (!_ShouldDrawProperty(property)) return;
            EditorGUI.PropertyField(position, property, label, includeChildren: true);
        }


        private bool _ShouldDrawProperty(SerializedProperty property) {
            Object[] targets = property.serializedObject.targetObjects;

            var showIfAttributes = fieldInfo
                .GetCustomAttributes(typeof(HShowIfAttribute), inherit: true)
                .Cast<HShowIfAttribute>()
                .ToArray();

            var hideIfAttributes = fieldInfo
                .GetCustomAttributes(typeof(HHideIfAttribute), inherit: true)
                .Cast<HHideIfAttribute>()
                .ToArray();

            // 1) HHideIf: OR within one target;
            // hide only if ALL selected targets request hide.
            if (hideIfAttributes.Length > 0) {
                bool allTargetsWantHide = true;

                foreach (Object selectedObject in targets) {
                    bool anyHideTrue = false;

                    foreach (var attr in hideIfAttributes) {
                        if (_EvaluateCondition(selectedObject, attr.MemberName, attr.Comparison, attr.CompareValue)) {
                            anyHideTrue = true;
                            break;
                        }
                    }

                    if (!anyHideTrue) {
                        allTargetsWantHide = false;
                        break;
                    }
                }

                if (allTargetsWantHide)
                    return false; // hide
            }

            // 2) HShowIf: AND within one target;
            // show only if ALL selected targets satisfy ALL conditions.
            if (showIfAttributes.Length > 0) {
                foreach (Object selected in targets) {
                    foreach (var attr in showIfAttributes) {
                        if (!_EvaluateCondition(selected, attr.MemberName, attr.Comparison, attr.CompareValue))
                            return false; // any failure hides for multi-edit safety
                    }
                }
            }

            return true;
        }

        private static bool _EvaluateCondition(object target, string memberName, HComparison compare, object rightHandValue) {
            if (target == null || string.IsNullOrEmpty(memberName))
                return true;

            object memberValue = _GetMemberValue(target, memberName);

            switch (compare) {
            case HComparison.IsTrue:
                return _ConvertToBool(memberValue);
            case HComparison.IsFalse:
                return !_ConvertToBool(memberValue);
            case HComparison.Equals:
                return _AreEqual(memberValue, rightHandValue);
            case HComparison.NotEquals:
                return !_AreEqual(memberValue, rightHandValue);
            case HComparison.Greater:
                return _CompareValues(memberValue, rightHandValue) > 0;
            case HComparison.GreaterOrEqual:
                return _CompareValues(memberValue, rightHandValue) >= 0;
            case HComparison.Less:
                return _CompareValues(memberValue, rightHandValue) < 0;
            case HComparison.LessOrEqual:
                return _CompareValues(memberValue, rightHandValue) <= 0;
            default:
                return true;
            }
        }

        private static object _GetMemberValue(object target, string memberPath) {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            object curr = target;

            foreach (string segment in memberPath.Split('.')) {
                if (curr == null)
                    return null;

                System.Type type = curr.GetType();

                FieldInfo field = type.GetField(segment, flags);
                if (field != null) { curr = field.GetValue(curr); continue; }

                PropertyInfo property = type.GetProperty(segment, flags);
                if (property != null) { curr = property.GetValue(curr); continue; }

                MethodInfo method = type.GetMethod(segment, flags, null, System.Type.EmptyTypes, null);
                if (method != null) { curr = method.Invoke(curr, null); continue; }

                return null; // not found
            }

            return curr;
        }

        private static bool _ConvertToBool(object value) {
            if (value is bool boolValue)
                return boolValue;
            if (value == null)
                return false;
            if (_IsNumber(value))
                return System.Convert.ToDouble(value) != 0.0;
            return true; // not null => truthy
        }

        private static bool _AreEqual(object left, object right) {
            if (left == null && right == null) return true;
            if (left == null || right == null) return false;
            if (left.GetType().IsEnum && right is int rightInt)
                return System.Convert.ToInt32(left).Equals(rightInt);
            return Equals(left, right);
        }

        private static int _CompareValues(object left, object right) {
            if (left == null || right == null) return 0;
            if (_IsNumber(left) && _IsNumber(right)) {
                double leftNum = System.Convert.ToDouble(left);
                double rightNum = System.Convert.ToDouble(right);
                return leftNum.CompareTo(rightNum);
            }
            try {
                return System.Collections.Comparer.Default.Compare(left, right);
            }
            catch {
                return 0;
            }
        }

        private static bool _IsNumber(object value) {
            if (value == null) return false;

            var typeCode = System.Type.GetTypeCode(value.GetType());
            switch (typeCode) {
            case System.TypeCode.Byte:
            case System.TypeCode.SByte:
            case System.TypeCode.Int16:
            case System.TypeCode.UInt16:
            case System.TypeCode.Int32:
            case System.TypeCode.UInt32:
            case System.TypeCode.Int64:
            case System.TypeCode.UInt64:
            case System.TypeCode.Single:
            case System.TypeCode.Double:
            case System.TypeCode.Decimal:
                return true;
            default:
                return false;
            }
        }
    }
}
#endif
