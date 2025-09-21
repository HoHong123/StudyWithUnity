using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Study.AS.Mvc {
    public sealed class MvcView : MonoBehaviour {
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
            if (incrementBtn != null)
                incrementBtn.onClick.RemoveListener(_OnIncrement);
        }

        private void _OnIncrement() => OnClickIncrement?.Invoke();
    }
}
