using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 해당 필드의 라벨 폭을 강제로 지정(픽셀 단위)하는 속성입니다.
     * 
     * ** 사용법 **
     * [HLabelWidth(120)]
     * =========================================================
     */
#endif
    /// <summary>
    /// Forces the label width of the field (in pixels).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HLabelWidthAttribute : PropertyAttribute {
        public readonly float Width;

        public HLabelWidthAttribute(float width) {
            this.Width = width;
        }
    }
}
