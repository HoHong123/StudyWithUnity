using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * Vector2(또는 Vector2Int)를 [min..max] 슬라이더로 그립니다.
     * - x = min, y = max. 한계값은 생성자 인자로 지정.
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HMinMaxSliderAttribute : PropertyAttribute {
        public readonly float limitMin;
        public readonly float limitMax;
        public readonly bool showFields;

        public HMinMaxSliderAttribute(float limitMin, float limitMax, bool showFields = true) {
            this.limitMin = Mathf.Min(limitMin, limitMax);
            this.limitMax = Mathf.Max(limitMin, limitMax);
            this.showFields = showFields;
        }
    }
}
