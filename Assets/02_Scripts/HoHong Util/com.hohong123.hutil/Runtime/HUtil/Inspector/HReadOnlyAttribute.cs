using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * HReadOnly �Ӽ� ��ũ��Ʈ�Դϴ�.
     * �ν����Ϳ��� ���� ǥ�ø� �ϰ� ���� �Ұ��� ����ϴ�.
     * ��� ����ȭ ������ �ʵ�/������Ƽ�� ��� �����մϴ�.
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HReadOnlyAttribute : PropertyAttribute {
        /// <summary>
        /// (����) �÷��� ��忡���� �б� �������� ����� ������ true.
        /// �⺻�� false = �׻� �б� ����.
        /// </summary>
        public readonly bool OnlyInPlayMode;

        public HReadOnlyAttribute() { }

        public HReadOnlyAttribute(bool onlyInPlayMode) {
            OnlyInPlayMode = onlyInPlayMode;
        }
    }
}