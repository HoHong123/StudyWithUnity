using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * Ư�� ������ ������ ��� �ʵ带 IMGUI�� ����ϴ� �Ӽ��Դϴ�.
     * 
     * ** ���� **
     * [HShowIf("isEnabled")] // bool�� ���
     * [HShowIf("Count", 5, HComparison.GreaterOrEqual)] // �� �񱳿���
     * [HShowIf("State", MyEnum.Ready)] // Enum ��
     * [HShowIf("IsValid")] // bool ��ȯ �Լ� ���
     * [HShowIf("IsVisible", HComparison.IsTrue)] // HComparasion ��ȯ �Լ� ���
     * =========================================================
     */
#endif
    /// <summary>
    /// Show the field if a condition is met.
    /// </summary>
    /// Examples:
    /// [HShowIf("isEnabled")] // expects bool member true
    /// [HShowIf("Count", 5, HComparison.GreaterOrEqual)] // Relational operator
    /// [HShowIf("State", MyEnum.Ready)] // enum comparison
    /// [HShowIf("IsValid")] // method bool IsValid()
    /// [HShowIf("IsVisible", HComparison.IsTrue)] // expects 'HComparison' member that equals with param
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class HShowIfAttribute : PropertyAttribute {
        public readonly string MemberName;
        public readonly HComparison Comparison;
        public readonly object CompareValue;

        public HShowIfAttribute(string memberName) {
            this.MemberName = memberName; 
            this.Comparison = HComparison.IsTrue; 
        }

        public HShowIfAttribute(string memberName, HComparison comparison) { 
            this.MemberName = memberName;
            this.Comparison = comparison;
        }

        public HShowIfAttribute(string memberName, object compareValue, HComparison comparison = HComparison.Equals) {
            this.MemberName = memberName; 
            this.CompareValue = compareValue; 
            this.Comparison = comparison; 
        }
    }

#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * Ư�� ������ ������ ��� �ʵ带 IMGUI���� ����� �Ӽ��Դϴ�.
     * 
     * ** ���� **
     * [HHideIf("isEnabled")] // bool�� ���
     * [HHideIf("Count", 5, HComparison.GreaterOrEqual)] // �� �񱳿���
     * [HHideIf("State", MyEnum.Ready)] // Enum ��
     * [HHideIf("IsValid")] // bool ��ȯ �Լ� ���
     * [HHideIf("IsVisible", HComparison.IsTrue)] // HComparasion ��ȯ �Լ� ���
     * =========================================================
     */
#endif
    /// <summary>
    /// Hides the field when the referenced member satisfies the condition.
    /// </summary>
    /// Examples:
    ///   [HHideIf("isEnabled")] // bool field/property/method() must be true
    ///   [HHideIf("health", 0, HComparison.Greater)]
    ///   [HHideIf("State", MyEnum.Ready)]
    ///   [HHideIf("IsValid", HComparison.IsTrue)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class HHideIfAttribute : PropertyAttribute {
        public readonly string MemberName;
        public readonly HComparison Comparison;
        public readonly object CompareValue;

        public HHideIfAttribute(string memberName) { 
            this.MemberName = memberName; 
            this.Comparison = HComparison.IsTrue; 
        }

        public HHideIfAttribute(string memberName, HComparison comparison) { 
            this.MemberName = memberName; 
            this.Comparison = comparison; 
        }

        public HHideIfAttribute(string memberName, object compareValue, HComparison comparison = HComparison.Equals) { 
            this.MemberName = memberName; 
            this.CompareValue = compareValue; 
            this.Comparison = comparison;
        }
    }
}
