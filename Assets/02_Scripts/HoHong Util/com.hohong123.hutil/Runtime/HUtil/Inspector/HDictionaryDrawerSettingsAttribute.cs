using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * SerializableDictionary ���� Drawer���� �Դϴ�.
     * - Ű/�� ��, �ߺ� Ű ��� ����
     * - �� ��: ���� ���(�⺻) �Ǵ� �ȼ� ����(����)
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HDictionaryDrawerSettingsAttribute : PropertyAttribute {
        public readonly string KeyLabel;
        public readonly string ValueLabel;
        public readonly bool AllowDuplicateKeys;

        /// <summary>Ű �÷� ����(0~1). �ȼ� ������ 0�̸� �� ���� ���(�⺻ 0.35)</summary>
        public readonly float KeyColumnRatio;
        /// <summary>Ű �÷� �ȼ� ���� �ʺ�(>0�̸� ���� ��� ���)</summary>
        public readonly float KeyColumnPixels;
        /// <summary>Ű/�� �÷� ���� ����(px)</summary>
        public readonly float ColumnSpacing;
        /// <summary>������ ���� ��ư ��(px)</summary>
        public readonly float RemoveButtonWidth;

        public HDictionaryDrawerSettingsAttribute(
            string keyLabel = "Key",
            string valueLabel = "Value",
            bool allowDuplicateKeys = false,
            float keyColumnRatio = 0.35f,
            float keyColumnPixels = 0f,
            float columnSpacing = 5f,
            float removeButtonWidth = 20f) {
            KeyLabel = string.IsNullOrWhiteSpace(keyLabel) ? "Key" : keyLabel;
            ValueLabel = string.IsNullOrWhiteSpace(valueLabel) ? "Value" : valueLabel;
            AllowDuplicateKeys = allowDuplicateKeys;
            KeyColumnRatio = Mathf.Clamp01(keyColumnRatio);
            KeyColumnPixels = Mathf.Max(0f, keyColumnPixels);
            ColumnSpacing = Mathf.Max(2f, columnSpacing);
            RemoveButtonWidth = Mathf.Max(18f, removeButtonWidth);
        }
    }
}
