#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HTitle 속성 스크립트입니다.
 * Header기능을 대체하기 위해 작성되었습니다.
 * =========================================================
 */
#endif
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HUtil.Inspector;

namespace Study.AS.Mvc {
    public sealed class MvcView : MonoBehaviour {
        [HTitle("UI")]
        [SerializeField]
        TMP_Text labelTxt;
        [SerializeField]
        Button incrementBtn;

        public event Action OnClickIncrement;

        public void SetValue(int value) {
            if (labelTxt != null) labelTxt.text = value.ToString();
        }

        private void Awake() {
            if (incrementBtn != null) incrementBtn.onClick.AddListener(_OnIncrement);
        }

        private void OnDestroy() {
            if (incrementBtn != null) incrementBtn.onClick.RemoveListener(_OnIncrement);
        }

        private void _OnIncrement() => OnClickIncrement?.Invoke();
    }
}
