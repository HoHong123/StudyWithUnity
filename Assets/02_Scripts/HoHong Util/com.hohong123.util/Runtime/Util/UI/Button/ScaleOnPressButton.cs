using UnityEngine;
using HUtil.UI.Entity;
using HUtil.Inspector;

namespace HUtil.UI.ButtonUI {
    [RequireComponent(typeof(DelegateButton))]
    public class ScaleOnPressButton : BaseOnPressButton {
        [HTitle("Target")]
        [SerializeField]
        ScalingUiEntity[] targets;


        public override void OnPointDown() {
            foreach (var target in targets)
                target.ChangeScale();
        }

        public override void OnPointUp() {
            foreach (var target in targets)
                target.Reset();
        }
    }
}
