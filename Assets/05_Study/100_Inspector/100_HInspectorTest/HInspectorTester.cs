using UnityEngine;
using HUtil.Inspector;

namespace Study.HInspector {
    public class HInspectorTester : MonoBehaviour {
        [HTitle("HMinMaxSlider")]
        [SerializeField]
        [HMinMaxSlider(0, 100)]
        Vector2 vector2Slider;
        [SerializeField]
        [HMinMaxSlider(0, 10)]
        int intSlider;
        [SerializeField]
        [HMinMaxSlider(0f, 100f)]
        float floatSlider;

        [HTitle("HReadOnly")]
        [SerializeField]
        [HReadOnly]
        int readOnly;

        [HTitle("HShowIf")]
        [SerializeField]
        bool showIf;
        [HShowIf("showIf")]
        [SerializeField]
        int showWhenTrue;

        [HTitle("HHideIf")]
        [SerializeField]
        bool hideIf;
        [HHideIf("hideIf")]
        [SerializeField]
        int hideWhenTrue;

        [HTitle("HOnValueChanged")]
        [SerializeField]
        [HReadOnly]
        int doubleUpValueChange = 0;
        [HOnValueChanged("_OnValueChange")]
        [SerializeField]
        int onValueChange = 0;

        private void _OnValueChange() {
            doubleUpValueChange = onValueChange * 2;
        }
    }
}
