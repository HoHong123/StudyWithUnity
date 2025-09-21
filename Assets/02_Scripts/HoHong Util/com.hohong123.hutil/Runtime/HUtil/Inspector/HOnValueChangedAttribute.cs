using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * �ν����Ϳ��� ���� �ٲ�� ���� �޼��带 ȣ���ϴ� �Ӽ��Դϴ�.
     * - �޼���� �Ű����� ���� ���� �Ǵ� ���� �Ű����� ���¸� ����.
     * - ���� �Ű������� ���, �� ��(�����ϸ� ����ȯ �ؼ�)�� ����.
     * - ���� ���� �޸� ���� ������� ���� ȣ��.
     * =========================================================
     */
#endif
    /// <summary>
    /// Call a specified method when a value changes in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class HOnValueChangedAttribute : PropertyAttribute {
        public readonly string MethodName;
        public readonly bool AlsoOnValidate; // OnValidate������ ȣ������ ����(�ɼ�)

        public HOnValueChangedAttribute(string methodName, bool alsoOnValidate = false) {
            this.MethodName = methodName;
            this.AlsoOnValidate = alsoOnValidate;
        }
    }
}
