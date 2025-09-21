using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * �ش� �ʵ��� ���� �ּҰ� �̻����� �����ϴ� �Ӽ��Դϴ�.
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
     * �ش� �ʵ��� ���� �ִ밪 ���Ϸ� �����ϴ� �Ӽ��Դϴ�.
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
