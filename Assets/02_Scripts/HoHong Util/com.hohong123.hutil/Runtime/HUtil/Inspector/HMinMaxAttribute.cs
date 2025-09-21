using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 해당 필드의 값을 최소값 이상으로 강제하는 속성입니다.
     * =========================================================
     */
#endif
    /// <summary>Force the value of the field to be greater than or equal to the minimum value</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HMinAttribute : PropertyAttribute {
        public readonly double MinValue;
        public HMinAttribute(double minValue) => this.MinValue = minValue;
    }

#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 해당 필드의 값을 최대값 이하로 강제하는 속성입니다.
     * =========================================================
     */
#endif
    /// <summary>Force the value of the field to be less than the maximum value</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HMaxAttribute : PropertyAttribute {
        public readonly double MaxValue;
        public HMaxAttribute(double maxValue) => this.MaxValue = maxValue;
    }
}
