using System;
using UnityEngine;

namespace HUtil.Inspector {
#if UNITY_EDITOR
    /* =========================================================
     * @Jason - PKH
     * SerializableDictionary 전용 Drawer설정 입니다.
     * - 키/값 라벨, 중복 키 허용 여부
     * - 열 폭: 비율 기반(기본) 또는 픽셀 고정(선택)
     * =========================================================
     */
#endif
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HDictionaryDrawerSettingsAttribute : PropertyAttribute {
        public readonly string KeyLabel;
        public readonly string ValueLabel;
        public readonly bool AllowDuplicateKeys;

        /// <summary>키 컬럼 비율(0~1). 픽셀 고정이 0이면 이 값을 사용(기본 0.35)</summary>
        public readonly float KeyColumnRatio;
        /// <summary>키 컬럼 픽셀 고정 너비(>0이면 비율 대신 사용)</summary>
        public readonly float KeyColumnPixels;
        /// <summary>키/값 컬럼 사이 간격(px)</summary>
        public readonly float ColumnSpacing;
        /// <summary>오른쪽 제거 버튼 폭(px)</summary>
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
