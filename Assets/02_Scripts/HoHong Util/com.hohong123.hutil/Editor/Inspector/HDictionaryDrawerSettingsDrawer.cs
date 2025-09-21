#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * HUtil.Inspector.SerializableDictionary 전용 Drawer입니다.
     * - 행 내부 라벨 제거(공간 확보), 헤더에만 Key/Data 라벨
     * - 키 컬럼: 비율 기반(기본) 또는 픽셀 고정
     * - 중복 키 경고(옵션)
     * =========================================================
     */
#endif
    [CustomPropertyDrawer(typeof(HUtil.Inspector.HDictionaryDrawerSettingsAttribute), useForChildren: true)]
    public class HDictionaryDrawerSettingsDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!_TryGetLists(property, out SerializedProperty keysProp, out SerializedProperty valuesProp))
                return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);

            int pairCount = Mathf.Min(keysProp.arraySize, valuesProp.arraySize);
            float height = EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight + 2f;

            for (int index = 0; index < pairCount; index++) {
                SerializedProperty keyElem = keysProp.GetArrayElementAtIndex(index);
                SerializedProperty valElem = valuesProp.GetArrayElementAtIndex(index);

                float rowHeight = Mathf.Max(
                    EditorGUI.GetPropertyHeight(keyElem, includeChildren: true),
                    EditorGUI.GetPropertyHeight(valElem, includeChildren: true)
                );
                height += rowHeight + 2f;
            }

            height += EditorGUIUtility.singleLineHeight + 4f; // 푸터(+ / Clear)
            // 중복 경고 줄은 필요 시 OnGUI에서 추가로 그려짐
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (!_TryGetLists(property, out SerializedProperty keysProp, out SerializedProperty valuesProp)) {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
                return;
            }

            var options = (HUtil.Inspector.HDictionaryDrawerSettingsAttribute)attribute;

            // 타이틀(필드 라벨)
            Rect titleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(titleRect,
                $"{label.text}  ({Mathf.Min(keysProp.arraySize, valuesProp.arraySize)})",
                EditorStyles.boldLabel);

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // 컬럼 폭 계산 (비율 우선, 픽셀 지정 시 픽셀 고정)
            float availableWidth = position.width;
            float keyWidth = options.KeyColumnPixels > 0f
                ? options.KeyColumnPixels
                : Mathf.Round(availableWidth * Mathf.Clamp01(options.KeyColumnRatio));

            float spacing = Mathf.Max(2f, options.ColumnSpacing);
            float removeWidth = Mathf.Max(18f, options.RemoveButtonWidth);

            // 컬럼 헤더 라인
            Rect headerRow = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect keyHeadRect = new Rect(headerRow.x, headerRow.y, keyWidth, headerRow.height);
            Rect valueHeadRect = new Rect(keyHeadRect.xMax + spacing, headerRow.y,
                                          headerRow.width - keyWidth - spacing - removeWidth - 4f, headerRow.height);
            Rect removeHeadRect = new Rect(valueHeadRect.xMax + 4f, headerRow.y, removeWidth, headerRow.height);

            using (new EditorGUI.DisabledScope(true)) {
                EditorGUI.LabelField(keyHeadRect, options.KeyLabel, EditorStyles.miniBoldLabel);
                EditorGUI.LabelField(valueHeadRect, options.ValueLabel, EditorStyles.miniBoldLabel);
                EditorGUI.LabelField(removeHeadRect, GUIContent.none);
            }
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            // 데이터 행
            int pairCount = Mathf.Min(keysProp.arraySize, valuesProp.arraySize);
            for (int index = 0; index < pairCount; index++) {
                SerializedProperty keyElem = keysProp.GetArrayElementAtIndex(index);
                SerializedProperty valElem = valuesProp.GetArrayElementAtIndex(index);

                float rowHeight = Mathf.Max(
                    EditorGUI.GetPropertyHeight(keyElem, includeChildren: true),
                    EditorGUI.GetPropertyHeight(valElem, includeChildren: true)
                );

                Rect rowRect = new Rect(position.x, position.y, position.width, rowHeight);
                Rect keyRect = new Rect(rowRect.x, rowRect.y, keyWidth, rowRect.height);
                Rect valueRect = new Rect(keyRect.xMax + spacing, rowRect.y,
                                          rowRect.width - keyWidth - spacing - removeWidth - 4f, rowRect.height);
                Rect removeRect = new Rect(valueRect.xMax + 4f, rowRect.y, removeWidth, EditorGUIUtility.singleLineHeight);

                int previousIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0; // 내부 들여쓰기 제거로 가로폭 확보

                // 라벨 없이 그려 공간 극대화
                EditorGUI.PropertyField(keyRect, keyElem, GUIContent.none, includeChildren: true);
                EditorGUI.PropertyField(valueRect, valElem, GUIContent.none, includeChildren: true);

                EditorGUI.indentLevel = previousIndent;

                if (GUI.Button(removeRect, "X")) {
                    keysProp.DeleteArrayElementAtIndex(index);
                    valuesProp.DeleteArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();
                    GUI.changed = true;
                    GUIUtility.ExitGUI();
                }

                position.y += rowHeight + 2f;
            }

            // 푸터(+ / Clear)
            Rect footer = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight + 2f);
            float buttonWidth = 60f;
            Rect addRect = new Rect(footer.x, footer.y, buttonWidth, footer.height);
            Rect clearRect = new Rect(addRect.xMax + 4f, footer.y, buttonWidth, footer.height);

            if (GUI.Button(addRect, "Add")) {
                int newIndex = keysProp.arraySize;
                keysProp.arraySize++;
                valuesProp.arraySize++;

                _InitializeNewElement(keysProp.GetArrayElementAtIndex(newIndex));
                _InitializeNewElement(valuesProp.GetArrayElementAtIndex(newIndex));

                property.serializedObject.ApplyModifiedProperties();
                GUI.changed = true;
                GUIUtility.ExitGUI();
            }
            if (GUI.Button(clearRect, "Clear")) {
                keysProp.ClearArray();
                valuesProp.ClearArray();
                property.serializedObject.ApplyModifiedProperties();
                GUI.changed = true;
                GUIUtility.ExitGUI();
            }

            // 중복 키 경고(옵션)
            if (!options.AllowDuplicateKeys && _HasDuplicateKeys(keysProp)) {
                Rect warnRect = new Rect(footer.x, footer.yMax + 2f, footer.width, EditorGUIUtility.singleLineHeight * 1.2f);
                EditorGUI.HelpBox(warnRect, "Duplicate keys detected.", MessageType.Warning);
            }
        }

        private static bool _TryGetLists(SerializedProperty property, out SerializedProperty keysProp, out SerializedProperty valuesProp) {
            keysProp = property.FindPropertyRelative("keys");
            valuesProp = property.FindPropertyRelative("values");
            return keysProp != null && valuesProp != null && keysProp.isArray && valuesProp.isArray;
        }

        private static bool _HasDuplicateKeys(SerializedProperty keysProp) {
            HashSet<string> set = new();
            for (int index = 0; index < keysProp.arraySize; index++) {
                SerializedProperty elem = keysProp.GetArrayElementAtIndex(index);
                string token = elem.propertyType switch {
                    SerializedPropertyType.Integer => elem.intValue.ToString(),
                    SerializedPropertyType.Boolean => elem.boolValue.ToString(),
                    SerializedPropertyType.Float => elem.doubleValue.ToString("R"),
                    SerializedPropertyType.String => elem.stringValue ?? "",
                    SerializedPropertyType.Enum => elem.enumValueIndex.ToString(),
                    SerializedPropertyType.ObjectReference => elem.objectReferenceInstanceIDValue.ToString(),
                    SerializedPropertyType.Vector2 => elem.vector2Value.ToString(),
                    SerializedPropertyType.Vector2Int => elem.vector2IntValue.ToString(),
                    SerializedPropertyType.Vector3 => elem.vector3Value.ToString(),
                    SerializedPropertyType.Vector3Int => elem.vector3IntValue.ToString(),
                    _ => elem.propertyType + ":" + elem.propertyPath
                };
                if (!set.Add(token)) return true;
            }
            return false;
        }

        private static void _InitializeNewElement(SerializedProperty element) {
            switch (element.propertyType) {
            case SerializedPropertyType.Integer:
                element.intValue = default;
                break;
            case SerializedPropertyType.Boolean:
                element.boolValue = default;
                break;
            case SerializedPropertyType.Float:
                element.doubleValue = default;
                break;
            case SerializedPropertyType.String:
                element.stringValue = string.Empty;
                break;
            case SerializedPropertyType.Enum:
                element.enumValueIndex = 0;
                break;
            case SerializedPropertyType.Color:
                element.colorValue = Color.white;
                break;
            case SerializedPropertyType.ObjectReference:
                element.objectReferenceValue = null;
                break;
            case SerializedPropertyType.Vector2:
                element.vector2Value = default;
                break;
            case SerializedPropertyType.Vector3:
                element.vector3Value = default;
                break;
            case SerializedPropertyType.Vector4:
                element.vector4Value = default;
                break;
            case SerializedPropertyType.Rect:
                element.rectValue = default;
                break;
            case SerializedPropertyType.Bounds:
                element.boundsValue = default;
                break;
            case SerializedPropertyType.Vector2Int:
                element.vector2IntValue = default;
                break;
            case SerializedPropertyType.Vector3Int:
                element.vector3IntValue = default;
                break;
            case SerializedPropertyType.RectInt:
                element.rectIntValue = default;
                break;
            case SerializedPropertyType.BoundsInt:
                element.boundsIntValue = default;
                break;
            case SerializedPropertyType.Quaternion:
                element.quaternionValue = Quaternion.identity;
                break;
            }
        }
    }
}
#endif
