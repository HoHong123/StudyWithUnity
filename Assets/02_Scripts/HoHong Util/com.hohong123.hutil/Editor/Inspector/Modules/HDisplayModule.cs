#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace HEditor.Inspector.Modules {
    public sealed class HDisplayModule : HBaseModule {
        public override bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp) {
            var field = _GetFieldInfo(target.GetType(), serProp.propertyPath);
            if (field == null) return false;

            var showIf = field.GetCustomAttribute<HUtil.Inspector.HShowIfAttribute>(inherit: true);
            var hideIf = field.GetCustomAttribute<HUtil.Inspector.HHideIfAttribute>(inherit: true);
            if (showIf == null && hideIf == null) return false;

            bool visible = _EvalVisibility(target, serProp, field, showIf, hideIf);
            if (!visible) return true;

            EditorGUILayout.PropertyField(serProp, true);
            return true;
        }


        private bool _EvalVisibility(Object target, SerializedProperty serProp, FieldInfo field, object showIf, object hideIf) {
            Type compType = Type.GetType("HUtil.Inspector.HComparison, Assembly-CSharp") ??
                           Type.GetType("HUtil.Inspector.HComparison");
            if (compType != null) {
                var method = compType.GetMethod("Evaluate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null) {
                    try {
                        var res = method.Invoke(null, new object[] { target, serProp, field, showIf, hideIf });
                        if (res is bool b) return b;
                    }
                    catch { 
                    }
                }
            }

            return true;
        }
    }
}
#endif
