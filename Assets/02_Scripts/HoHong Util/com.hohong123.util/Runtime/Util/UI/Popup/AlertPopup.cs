using UnityEngine;
using TMPro;
using PoolReturn = HUtil.Pooling.IPoolReturn<HUtil.UI.Popup.AlertPopup>;
using PoolDispose = HUtil.Pooling.IPoolDispose<HUtil.UI.Popup.AlertPopup>;
using HUtil.Inspector;

namespace HUtil.UI.Popup {
    public class AlertPopup : BasePopupUi, PoolReturn, PoolDispose {
        [HTitle("Texts")]
        [SerializeField]
        TMP_Text titleTxt;
        [SerializeField]
        TMP_Text descriptionTxt;


        public void SetUi(string title, string message) {
            titleTxt.text = title;
            descriptionTxt.text = message;
        }


        public void OnReturn(AlertPopup mono) {
            mono.titleTxt.text = string.Empty;
            mono.descriptionTxt.text = string.Empty;
        }

        public void OnDispose(AlertPopup mono) {
            Destroy(mono.panel);
        }
    }
}