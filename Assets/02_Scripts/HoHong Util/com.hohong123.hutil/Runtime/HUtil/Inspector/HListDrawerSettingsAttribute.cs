using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 리스트/배열 필드의 그리기 동작을 제어합니다.
     * - Unity의 ReorderableList 기반(에디터).
     * - 페이징 옵션 사용 시, 드래그 정렬은 비활성(기술적 제약).
     * TODO :: Fix require
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HListDrawerSettingsAttribute : PropertyAttribute {
        /// <summary>드래그로 요소 순서 변경 허용(페이징이 꺼져 있을 때만 의미 있음)</summary>
        public readonly bool IsDraggable;
        /// <summary>추가 버튼 표시</summary>
        public readonly bool ShowAddButton;
        /// <summary>삭제 버튼 표시</summary>
        public readonly bool ShowRemoveButton;
        /// <summary>헤더 영역 숨김</summary>
        public readonly bool HideHeader;
        /// <summary>항목 좌측에 인덱스 라벨(0,1,2..) 표시</summary>
        public readonly bool ShowIndexLabels;
        /// <summary>초기 확장 상태</summary>
        public readonly bool StartExpanded;
        /// <summary>페이징 사용 여부</summary>
        public readonly bool UsePaging;
        /// <summary>페이지당 항목 수(usePaging=true 일 때만 사용)</summary>
        public readonly int PageSize;
        /// <summary>추가 버튼 레이블(툴바/헤더 등 커스텀 UI에 사용)</summary>
        public readonly string AddLabel;
        /// <summary>삭제 버튼 레이블</summary>
        public readonly string RemoveLabel;

        public HListDrawerSettingsAttribute(
            bool isDraggable = true,
            bool showAddButton = true,
            bool showRemoveButton = true,
            bool hideHeader = false,
            bool showIndexLabels = false,
            bool startExpanded = true,
            bool usePaging = true,
            int pageSize = 10,
            string addLabel = "+",
            string removeLabel = "−") {
            IsDraggable = isDraggable;
            ShowAddButton = showAddButton;
            ShowRemoveButton = showRemoveButton;
            HideHeader = hideHeader;
            ShowIndexLabels = showIndexLabels;
            StartExpanded = startExpanded;
            UsePaging = usePaging;
            PageSize = Mathf.Max(1, pageSize);
            AddLabel = string.IsNullOrEmpty(addLabel) ? "+" : addLabel;
            RemoveLabel = string.IsNullOrEmpty(removeLabel) ? "−" : removeLabel;
        }
    }
}
