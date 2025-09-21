using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * 인스펙터에서 값이 바뀌면 지정 메서드를 호출하는 속성입니다.
     * - 메서드는 매개변수 없는 형태 또는 단일 매개변수 형태를 지원.
     * - 단일 매개변수일 경우, 새 값(가능하면 형변환 해서)을 전달.
     * - 여러 개를 달면 선언 순서대로 전부 호출.
     * =========================================================
     */
#endif
    /// <summary>
    /// Call a specified method when a value changes in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class HOnValueChangedAttribute : PropertyAttribute {
        public readonly string MethodName;
        public readonly bool AlsoOnValidate; // OnValidate에서도 호출할지 여부(옵션)

        public HOnValueChangedAttribute(string methodName, bool alsoOnValidate = false) {
            this.MethodName = methodName;
            this.AlsoOnValidate = alsoOnValidate;
        }
    }
}
