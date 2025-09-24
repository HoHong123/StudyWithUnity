#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public sealed class HReadOnlyModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var attr = field.GetCustomAttribute<HUtil.Inspector.HTitleAttribute>(inherit: true);
            if (attr == null) return false;

            using (new EditorGUI.DisabledScope(true)) {
                EditorGUILayout.PropertyField(serProp, true);
            }
            return true;
        }
    }
}
#endif
