#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public sealed class HLabelWidthModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var attr = field.GetCustomAttribute<HUtil.Inspector.HLabelWidthAttribute>(inherit: true);
            if (attr == null) return false;

            float old = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = Mathf.Max(0f, attr.Width);
            EditorGUILayout.PropertyField(serProp, true);
            EditorGUIUtility.labelWidth = old;
            return true;
        }
    }
}
#endif
