using UnityEngine;
using HUtil.UI.Entity;
using HUtil.Inspector;

namespace HUtil.UI.ButtonUI {
    [RequireComponent(typeof(DelegateButton))]
    public class ColorOnPressButton : BaseOnPressButton {
        [HTitle("Target")]
        [SerializeField]
        ColorUiEntity[] targets;

        public ColorUiEntity[] ColorEntity => targets;


        public override void OnPointDown() {
            foreach (var target in targets)
                target.Dye();
        }

        public override void OnPointUp() {
            foreach (var target in targets)
                target.Reset();
        }
    }
}
