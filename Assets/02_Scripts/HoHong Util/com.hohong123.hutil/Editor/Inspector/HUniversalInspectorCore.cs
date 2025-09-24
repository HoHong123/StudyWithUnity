#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HEditor.Inspector.Modules;

namespace HEditor.Inspector {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class HUniversalInspectorMono : Editor {
        public override void OnInspectorGUI() => HUniversalInspectorCore.Draw(serializedObject, target);
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class HUniversalInspectorSO : Editor {
        public override void OnInspectorGUI() => HUniversalInspectorCore.Draw(serializedObject, target);
    }

    public static class HUniversalInspectorCore {
        static readonly List<IInspectorModule> modules = new();

        static HUniversalInspectorCore() {
            // 등록 순서 = 우선순위
            modules.Add(new HTitleModule());
            modules.Add(new HDisplayModule());
            modules.Add(new HReadOnlyModule());
            modules.Add(new HLabelWidthModule());
            modules.Add(new HOnValueChangedModule());
            modules.Add(new HLimitModule());
            modules.Add(new HMinMaxSliderModule());
            //modules.Add(new HListModule());
            //modules.Add(new HDictionaryModule());
        }

        public static void Draw(SerializedObject so, Object targetObject) {
            if (so == null) return;
            EditorGUILayout.HelpBox("HUniversalInspector ACTIVE", MessageType.Info);
            so.Update();

            using (new EditorGUI.DisabledScope(true)) {
                var scriptProp = so.FindProperty("m_Script");
                if (scriptProp != null) EditorGUILayout.PropertyField(scriptProp, true);
            }

            var iter = so.GetIterator();
            bool enterChildren = true;

            while (iter.NextVisible(enterChildren)) {
                enterChildren = false;
                if (iter.propertyPath == "m_Script") continue;

                bool handled = false;
                for (int k = 0; k < modules.Count; k++) {
                    if (modules[k].TryConsume(so, targetObject, ref iter)) {
                        handled = true;
                        break;
                    }
                }

                if (!handled) {
                    EditorGUILayout.PropertyField(iter, true);
                }
            }

            so.ApplyModifiedProperties();
        }
    }
}
#endif
