//#if UNITY_EDITOR
///* =========================================================
// * @Jason - PKH
// * HListDrawerSettings 전용 드로워입니다.
// * - 배열/리스트만 지원. 문자열은 제외.
// * - 멀티 오브젝트 편집은 Unity 한계 때문에 기본 PropertyField로 폴백한다.
// * - HHideLabel, HLabelWidth 존중(유틸 호출).
// * - usePaging=true면 수동 렌더링(드래그 정렬 비활성).
// * =========================================================
// */
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEditorInternal;
//using HUtil.Inspector;

//namespace HEditor.Inspector {
//    [CustomPropertyDrawer(typeof(HListDrawerSettingsAttribute), useForChildren: true)]
//    public class HListDrawerSettingsDrawer : PropertyDrawer {
//        private sealed class CachedListData {
//            public ReorderableList reorderableList;
//            public bool isExpanded;
//            public int currentPage;
//            public HListDrawerSettingsAttribute options;
//        }

//        // key: instanceID | propertyPath
//        private static readonly Dictionary<string, CachedListData> cache =
//            new Dictionary<string, CachedListData>(256);

//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
//            if (!_IsArrayOrList(property))
//                return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);

//            // 멀티-오브젝트 편집은 안전하게 기본 동작
//            if (property.serializedObject.isEditingMultipleObjects)
//                return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);

//            var key = _MakeKey(property);
//            var cached = _GetOrBuildCache(property, key);

//            // 접힌 상태면 한 줄만
//            if (!cached.isExpanded)
//                return EditorGUIUtility.singleLineHeight;

//            if (!cached.options.UsePaging && cached.reorderableList != null) {
//                // ReorderableList가 높이를 계산
//                return cached.reorderableList.GetHeight();
//            }

//            // 페이징 수동 모드: 헤더 + 요소들 + 푸터 버튼 영역
//            int totalCount = property.arraySize;
//            int pageSize = cached.options.PageSize;
//            int startIndex = Mathf.Clamp(cached.currentPage * pageSize, 0, totalCount);
//            int endIndex = Mathf.Min(startIndex + pageSize, totalCount);

//            float height = 0f;

//            // 헤더(툴바) + 제목
//            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//            // 요소들
//            for (int elementIndex = startIndex; elementIndex < endIndex; elementIndex++) {
//                var element = property.GetArrayElementAtIndex(elementIndex);
//                height += EditorGUI.GetPropertyHeight(element, includeChildren: true) + 2f;
//            }

//            // 푸터(추가/삭제)
//            height += EditorGUIUtility.singleLineHeight + 4f;

//            return height;
//        }

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//            // 배열/리스트가 아니면 기본 필드
//            if (!_IsArrayOrList(property)) {
//                EditorGUI.PropertyField(position, property, label, includeChildren: true);
//                return;
//            }

//            // 멀티-오브젝트 편집은 폴백
//            if (property.serializedObject.isEditingMultipleObjects) {
//                EditorGUI.PropertyField(position, property, label, includeChildren: true);
//                return;
//            }

//            var key = _MakeKey(property);
//            var cached = _GetOrBuildCache(property, key);

//            // 헤더 라벨 유틸 적용
//            using (HDisplayLabelWidthUtil.UseIfAny(fieldInfo)) {
//                label = HDisplayLabelUtil.ResolveLabel(fieldInfo, label);
//                _DrawList(position, property, label, cached);
//            }
//        }


//        private void _DrawList(Rect rect, SerializedProperty property, GUIContent label, CachedListData cached) {
//            // 접기/펼치기 토글
//            Rect foldoutRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
//            if (!ReferenceEquals(label, GUIContent.none)) {
//                cached.isExpanded = EditorGUI.Foldout(foldoutRect, cached.isExpanded, label, true);
//            }
//            else {
//                // 라벨이 없으면 클릭할 foldout이 없으니 항상 펼침 유지
//                cached.isExpanded = true;
//            }

//            if (!cached.isExpanded)
//                return;

//            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            rect.height -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//            if (!cached.options.UsePaging) {
//                // ReorderableList 경로
//                _EnsureReorderableList(property, cached);
//                if (cached.reorderableList != null) {
//                    cached.reorderableList.DoList(rect);
//                }
//                return;
//            }

//            // --- 페이징 수동 경로 ---
//            _DrawPagedList(rect, property, cached);
//        }

//        private void _DrawPagedList(Rect rect, SerializedProperty property, CachedListData cached) {
//            int totalCount = property.arraySize;
//            int pageSize = cached.options.PageSize;
//            int totalPages = Mathf.Max(1, Mathf.CeilToInt(totalCount / (float)pageSize));
//            cached.currentPage = Mathf.Clamp(cached.currentPage, 0, totalPages - 1);

//            // 헤더(툴바)
//            Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
//            using (new EditorGUI.DisabledScope(cached.options.HideHeader)) {
//                _DrawToolbar(toolbarRect, property, cached, totalPages);
//            }

//            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            rect.height -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

//            // 요소들
//            int startIndex = Mathf.Clamp(cached.currentPage * pageSize, 0, totalCount);
//            int endIndex = Mathf.Min(startIndex + pageSize, totalCount);

//            for (int elementIndex = startIndex; elementIndex < endIndex; elementIndex++) {
//                var element = property.GetArrayElementAtIndex(elementIndex);

//                float elementHeight = EditorGUI.GetPropertyHeight(element, includeChildren: true);
//                Rect elementRect = new Rect(rect.x, rect.y, rect.width, elementHeight);

//                _DrawElementWithOptionalIndex(elementRect, element, elementIndex, cached.options.ShowIndexLabels);

//                rect.y += elementHeight + 2f;
//                rect.height -= elementHeight + 2f;
//            }

//            // 푸터(추가/삭제)
//            Rect footerRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight + 2f);
//            _DrawFooter(footerRect, property, cached.options);
//        }

//        private void _DrawToolbar(Rect rect, SerializedProperty property, CachedListData cached, int totalPages) {
//            Rect leftRect = rect;
//            leftRect.width = Mathf.Max(60f, rect.width * 0.5f);

//            Rect rightRect = rect;
//            rightRect.x = leftRect.xMax + 4f;
//            rightRect.width = rect.width - leftRect.width - 4f;

//            using (new EditorGUILayout.HorizontalScope()) { }

//            using (new EditorGUI.DisabledScope(true)) {
//                string title = $"{property.displayName}  ({property.arraySize})";
//                EditorGUI.LabelField(leftRect, title, EditorStyles.boldLabel);
//            }

//            using (new EditorGUI.DisabledScope(totalPages <= 1)) {
//                int current = cached.currentPage + 1;
//                int newPage = current;

//                float buttonWidth = 26f;
//                Rect prevRect = new Rect(rightRect.x, rightRect.y, buttonWidth, rightRect.height);
//                Rect pageRect = new Rect(prevRect.xMax + 4f, prevRect.y, 60f, prevRect.height);
//                Rect nextRect = new Rect(pageRect.xMax + 4f, prevRect.y, buttonWidth, prevRect.height);

//                if (GUI.Button(prevRect, "<")) newPage = Mathf.Max(1, current - 1);
//                newPage = EditorGUI.IntField(pageRect, newPage);
//                newPage = Mathf.Clamp(newPage, 1, totalPages);
//                if (GUI.Button(nextRect, ">")) newPage = Mathf.Min(totalPages, newPage + 1);
//                if (newPage != current) cached.currentPage = newPage - 1;
//            }
//        }

//        private void _DrawFooter(Rect rect, SerializedProperty property, HListDrawerSettingsAttribute options) {
//            float buttonWidth = 60f;
//            Rect addRect = new Rect(rect.x, rect.y, buttonWidth, rect.height);
//            Rect removeRect = new Rect(addRect.xMax + 4f, rect.y, buttonWidth, rect.height);

//            if (options.ShowAddButton && GUI.Button(addRect, string.IsNullOrEmpty(options.AddLabel) ? "+" : options.AddLabel)) {
//                int newIndex = property.arraySize;
//                property.arraySize++;
//                var element = property.GetArrayElementAtIndex(newIndex);
//                _InitializeNewElement(element);
//                property.serializedObject.ApplyModifiedProperties();
//            }

//            if (options.ShowRemoveButton && GUI.Button(removeRect, string.IsNullOrEmpty(options.RemoveLabel) ? "−" : options.RemoveLabel)) {
//                if (property.arraySize > 0) {
//                    property.arraySize--;
//                    property.serializedObject.ApplyModifiedProperties();
//                }
//            }
//        }

//        private void _EnsureReorderableList(SerializedProperty property, CachedListData cached) {
//            if (cached.reorderableList != null) return;

//            var options = cached.options;
//            var list = new ReorderableList(property.serializedObject, property,
//                                           draggable: options.IsDraggable,
//                                           displayHeader: !options.HideHeader,
//                                           displayAddButton: options.ShowAddButton,
//                                           displayRemoveButton: options.ShowRemoveButton);

//            list.drawHeaderCallback = rect => {
//                string title = $"{property.displayName}  ({property.arraySize})";
//                EditorGUI.LabelField(rect, title, EditorStyles.boldLabel);
//            };

//            list.drawElementCallback = (rect, index, isActive, isFocused) => {
//                if (index < 0 || index >= property.arraySize)
//                    return;

//                var element = property.GetArrayElementAtIndex(index);
//                _DrawElementWithOptionalIndex(rect, element, index, options.ShowIndexLabels);
//            };

//            list.elementHeightCallback = index => {
//                if (index < 0 || index >= property.arraySize)
//                    return EditorGUIUtility.singleLineHeight;
//                var element = property.GetArrayElementAtIndex(index);
//                return EditorGUI.GetPropertyHeight(element, includeChildren: true) + 2f;
//            };

//            list.onAddCallback = l => {
//                int newIndex = property.arraySize;
//                property.arraySize++;
//                var element = property.GetArrayElementAtIndex(newIndex);
//                _InitializeNewElement(element);
//                property.serializedObject.ApplyModifiedProperties();
//            };

//            list.onRemoveCallback = l => {
//                if (property.arraySize > 0) {
//                    property.arraySize--;
//                    property.serializedObject.ApplyModifiedProperties();
//                }
//            };

//            cached.reorderableList = list;
//        }

//        private void _DrawElementWithOptionalIndex(Rect rect, SerializedProperty element, int index, bool showIndex) {
//            if (showIndex) {
//                const float indexWidth = 30f;
//                Rect indexRect = new Rect(rect.x, rect.y, indexWidth, EditorGUIUtility.singleLineHeight);
//                EditorGUI.LabelField(indexRect, index.ToString(), EditorStyles.miniLabel);
//                Rect valueRect = new Rect(rect.x + indexWidth + 4f, rect.y, rect.width - indexWidth - 4f, rect.height);
//                EditorGUI.PropertyField(valueRect, element, includeChildren: true);
//            }
//            else {
//                EditorGUI.PropertyField(rect, element, includeChildren: true);
//            }
//        }

//        private static void _InitializeNewElement(SerializedProperty element) {
//            switch (element.propertyType) {
//            case SerializedPropertyType.Integer:
//                element.intValue = default;
//                break;
//            case SerializedPropertyType.Boolean:
//                element.boolValue = default;
//                break;
//            case SerializedPropertyType.Float:
//                element.doubleValue = default;
//                break;
//            case SerializedPropertyType.String:
//                element.stringValue = string.Empty;
//                break;
//            case SerializedPropertyType.Enum:
//                element.enumValueIndex = 0;
//                break;
//            case SerializedPropertyType.Color:
//                element.colorValue = Color.white;
//                break;
//            case SerializedPropertyType.ObjectReference:
//                element.objectReferenceValue = null;
//                break;
//            case SerializedPropertyType.Vector2:
//                element.vector2Value = default;
//                break;
//            case SerializedPropertyType.Vector3:
//                element.vector3Value = default;
//                break;
//            case SerializedPropertyType.Vector4:
//                element.vector4Value = default;
//                break;
//            case SerializedPropertyType.Rect:
//                element.rectValue = default;
//                break;
//            case SerializedPropertyType.Bounds:
//                element.boundsValue = default;
//                break;
//            case SerializedPropertyType.Vector2Int:
//                element.vector2IntValue = default;
//                break;
//            case SerializedPropertyType.Vector3Int:
//                element.vector3IntValue = default;
//                break;
//            case SerializedPropertyType.RectInt:
//                element.rectIntValue = default;
//                break;
//            case SerializedPropertyType.BoundsInt:
//                element.boundsIntValue = default;
//                break;
//            case SerializedPropertyType.Quaternion:
//                element.quaternionValue = Quaternion.identity;
//                break;
//            default:
//                // 그 외: 중첩 구조체/클래스는 Unity 기본 초기화 규칙을 따름
//                break;
//            }
//        }

//        private static bool _IsArrayOrList(SerializedProperty property) {
//            // Unity 기준: isArray=true && string 제외 => 배열/리스트
//            return property.isArray && property.propertyType != SerializedPropertyType.String;
//        }

//        private static string _MakeKey(SerializedProperty property) {
//            int id = property.serializedObject.targetObject != null
//                ? property.serializedObject.targetObject.GetInstanceID()
//                : 0;
//            return id + "|" + property.propertyPath;
//        }

//        private CachedListData _GetOrBuildCache(SerializedProperty property, string key) {
//            if (cache.TryGetValue(key, out var cached)) {
//                cached.options = (HListDrawerSettingsAttribute)attribute;
//                return cached;
//            }

//            var options = (HListDrawerSettingsAttribute)attribute;

//            cached = new CachedListData {
//                options = options,
//                isExpanded = options.StartExpanded,
//                currentPage = 0,
//                reorderableList = null
//            };

//            cache[key] = cached;
//            return cached;
//        }
//    }
//}
//#endif
