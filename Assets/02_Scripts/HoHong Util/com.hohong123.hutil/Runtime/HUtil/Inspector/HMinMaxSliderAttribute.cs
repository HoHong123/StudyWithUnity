using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * Vector2(�Ǵ� Vector2Int)�� [min..max] �����̴��� �׸��ϴ�.
     * - x = min, y = max. �Ѱ谪�� ������ ���ڷ� ����.
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
