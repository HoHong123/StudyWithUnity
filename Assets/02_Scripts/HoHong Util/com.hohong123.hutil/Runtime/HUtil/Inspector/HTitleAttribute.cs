using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * HTitle 속성 스크립트입니다.
     * Header기능을 대체하기 위해 작성되었습니다.
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class HTitleAttribute : PropertyAttribute {
        public enum Align { 
            Left,
            Center
        }

        public readonly string Title;
        public readonly string Subtitle;
        public readonly bool Bold;
        public readonly bool HorizontalLine;
        public readonly Align Alignment;
        public readonly float SpaceBefore;
        public readonly float SpaceAfter;

        public HTitleAttribute(string title) : this(title, null, true, true, Align.Left, 6f, 6f) { }

        public HTitleAttribute(string title, string subtitle, bool boldTitle = true, bool horizontalLine = true)
            : this(title, subtitle, boldTitle, horizontalLine, Align.Left, 6f, 6f) { }

        public HTitleAttribute(string title, string subtitle, bool boldTitle, bool horizontalLine, Align alignment, float spaceBefore = 6f, float spaceAfter = 6f) {
            Title = title;
            Subtitle = subtitle;
            Bold = boldTitle;
            HorizontalLine = horizontalLine;
            Alignment = alignment;
            SpaceBefore = Mathf.Max(0f, spaceBefore);
            SpaceAfter = Mathf.Max(0f, spaceAfter);
        }
    }
}
