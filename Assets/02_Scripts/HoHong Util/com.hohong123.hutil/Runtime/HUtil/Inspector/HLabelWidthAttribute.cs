using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * �ش� �ʵ��� �� ���� ������ ����(�ȼ� ����)�ϴ� �Ӽ��Դϴ�.
     * 
     * ** ���� **
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
