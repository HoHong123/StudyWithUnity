using System;
using UnityEngine;
using UnityEngine.UI;
using HUtil.Inspector;
using HUtil.UI.Panel;

namespace HUtil.UI.Popup {
    public class BasePopupUi : MonoBehaviour, IBasicPanel {
        [HTitle("Panel")]
        [SerializeField]
        protected GameObject panel;

        [HTitle("UI")]
        [SerializeField]
        protected Button closeBtn;

        public event Action OnClickCancel;

        public bool IsActive => panel.activeSelf;


        protected virtual void Start() {
            OnClickCancel += Close;
            closeBtn.onClick.AddListener(() => OnClickCancel?.Invoke());
        }


        public virtual void Open() => panel.SetActive(true);
        public virtual void Close() => panel.SetActive(false);
    }
}