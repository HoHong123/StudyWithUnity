//#if UNITY_EDITOR
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;

//namespace HEditor.Inspector.Modules {
//    /// <summary>[HBoxGroup]가 붙은 '연속된' 필드를 하나의 박스로 묶어 그린다.</summary>
//    public sealed class HBoxGroupModule : IInspectorModule {
//        private static readonly Dictionary<(int, string), bool> _groupOpen = new(); // (instanceID, groupId) -> isOpen

//        public bool TryConsume(SerializedObject so, UnityEngine.Object target, ref SerializedProperty iterator) {
//            var block = _BuildBlockIfAny(so, target, iterator);
//            if (block == null)
//                return false;

//            _DrawBlock(block, so);
//            if (!string.IsNullOrEmpty(block.lastPath))
//                iterator = so.FindProperty(block.lastPath);
//            return true;
//        }

//        // ----- private helpers -----

//        private sealed class _Block {
//            public string id;
//            public string title;
//            public bool collapsible;
//            public bool isOpen;
//            public readonly List<string> paths = new();
//            public string lastPath;
//        }

//        private _Block _BuildBlockIfAny(SerializedObject so, UnityEngine.Object target, SerializedProperty start) {
//            var fi = _GetFieldInfo(target.GetType(), start.propertyPath);
//            var attr = fi?.GetCustomAttribute<HUtil.Inspector.HBoxGroupAttribute>(inherit: true);
//            if (attr == null)
//                return null;

//            string groupId = attr.groupId;
//            string title = string.IsNullOrEmpty(attr.label) ? groupId : attr.label;

//            int instId = so.targetObject != null ? so.targetObject.GetInstanceID() : 0;
//            if (!_groupOpen.TryGetValue((instId, groupId), out bool isOpen))
//                isOpen = true;

//            var block = new _Block {
//                id = groupId,
//                title = title,
//                collapsible = attr.collapsible,
//                isOpen = isOpen
//            };

//            var walker = so.FindProperty(start.propertyPath);
//            do {
//                var wfi = _GetFieldInfo(target.GetType(), walker.propertyPath);
//                var wa = wfi?.GetCustomAttribute<HUtil.Inspector.HBoxGroupAttribute>(inherit: true);
//                if (wa == null || wa.groupId != groupId)
//                    break;

//                block.paths.Add(walker.propertyPath);
//                block.lastPath = walker.propertyPath;

//            } while (walker.NextVisible(false));

//            return block;
//        }

//        private void _DrawBlock(_Block block, SerializedObject so) {
//            int instId = so.targetObject != null ? so.targetObject.GetInstanceID() : 0;

//            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//            {
//                if (block.collapsible) {
//                    bool open = EditorGUILayout.Foldout(block.isOpen, block.title, true, EditorStyles.foldoutHeader);
//                    if (open != block.isOpen) {
//                        block.isOpen = open;
//                        _groupOpen[(instId, block.id)] = open;
//                    }
//                }
//                else {
//                    EditorGUILayout.LabelField(block.title, EditorStyles.boldLabel);
//                    block.isOpen = true;
//                    _groupOpen[(instId, block.id)] = true;
//                }

//                if (block.isOpen) {
//                    EditorGUI.indentLevel++;
//                    foreach (string path in block.paths) {
//                        var sp = so.FindProperty(path);
//                        if (sp != null)
//                            EditorGUILayout.PropertyField(sp, true);
//                    }
//                    EditorGUI.indentLevel--;
//                }
//            }
//            EditorGUILayout.EndVertical();
//            GUILayout.Space(2f);
//        }

//        private FieldInfo _GetFieldInfo(Type hostType, string propertyPath) {
//            if (hostType == null || string.IsNullOrEmpty(propertyPath))
//                return null;

//            string[] raw = propertyPath.Replace(".Array.data[", "[").Split('.');
//            var segments = new List<string>(raw.Length);
//            foreach (string seg in raw) {
//                if (string.IsNullOrEmpty(seg))
//                    continue;
//                int bracket = seg.IndexOf('[');
//                segments.Add(bracket >= 0 ? seg.Substring(0, bracket) : seg);
//            }

//            Type curr = hostType;
//            FieldInfo found = null;
//            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

//            for (int i = 0; i < segments.Count; i++) {
//                string name = segments[i];
//                found = curr.GetField(name, flags);
//                if (found == null)
//                    return null;

//                if (i < segments.Count - 1) {
//                    curr = found.FieldType;
//                    if (typeof(IList).IsAssignableFrom(curr) && curr.IsGenericType)
//                        curr = curr.GetGenericArguments()[0];
//                }
//            }
//            return found;
//        }
//    }
//}
//#endif
