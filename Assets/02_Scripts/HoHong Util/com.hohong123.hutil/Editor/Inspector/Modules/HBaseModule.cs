#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector.Modules {
    public abstract class HBaseModule : IInspectorModule {
        public abstract bool TryConsume(SerializedObject so, Object target, ref SerializedProperty serProp);

        protected virtual FieldInfo _GetFieldInfo(System.Type hostType, string path) {
            string[] raw = path.Replace(".Array.data[", "[").Split('.');
            var segs = new System.Collections.Generic.List<string>(raw.Length);
            foreach (var str in raw) {
                if (string.IsNullOrEmpty(str)) continue;
                int index = str.IndexOf('[');
                segs.Add((index >= 0) ? str.Substring(0, index) : str);
            }

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var curr = hostType;
            FieldInfo found = null;
            for (int k = 0; k < segs.Count; k++) {
                found = curr.GetField(segs[k], flags);
                if (found == null) return null;
                if (k < segs.Count - 1) {
                    curr = found.FieldType;
                    if (typeof(System.Collections.IList).IsAssignableFrom(curr) && curr.IsGenericType)
                        curr = curr.GetGenericArguments()[0];
                }
            }
            return found;
        }
    }
}
#endif
