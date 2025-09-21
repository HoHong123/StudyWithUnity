using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 특정 조건을 만족할 경우 필드를 IMGUI에 출력하는 속성입니다.
     * 
     * ** 사용법 **
     * [HShowIf("isEnabled")] // bool값 사용
     * [HShowIf("Count", 5, HComparison.GreaterOrEqual)] // 값 비교연산
     * [HShowIf("State", MyEnum.Ready)] // Enum 비교
     * [HShowIf("IsValid")] // bool 반환 함수 사용
     * [HShowIf("IsVisible", HComparison.IsTrue)] // HComparasion 반환 함수 사용
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
     * 특정 조건을 만족할 경우 필드를 IMGUI에서 숨기는 속성입니다.
     * 
     * ** 사용법 **
     * [HHideIf("isEnabled")] // bool값 사용
     * [HHideIf("Count", 5, HComparison.GreaterOrEqual)] // 값 비교연산
     * [HHideIf("State", MyEnum.Ready)] // Enum 비교
     * [HHideIf("IsValid")] // bool 반환 함수 사용
     * [HHideIf("IsVisible", HComparison.IsTrue)] // HComparasion 반환 함수 사용
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
