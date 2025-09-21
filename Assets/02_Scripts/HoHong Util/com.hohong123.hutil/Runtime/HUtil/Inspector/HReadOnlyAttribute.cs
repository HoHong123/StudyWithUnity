using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * HReadOnly 속성 스크립트입니다.
     * 인스펙터에서 값을 표시만 하고 수정 불가로 만듭니다.
     * 모든 직렬화 가능한 필드/프로퍼티에 사용 가능합니다.
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HReadOnlyAttribute : PropertyAttribute {
        /// <summary>
        /// (선택) 플레이 모드에서만 읽기 전용으로 만들고 싶으면 true.
        /// 기본값 false = 항상 읽기 전용.
        /// </summary>
        public readonly bool OnlyInPlayMode;

        public HReadOnlyAttribute() { }

        public HReadOnlyAttribute(bool onlyInPlayMode) {
            OnlyInPlayMode = onlyInPlayMode;
        }
    }
}