using System;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using HUtil.Inspector;


namespace HUtil.UI.Entity {
    [Serializable]
    public partial class ScalingUiEntity {
#if !ODIN_INSPECTOR
        [HTitle("Target")]
        [SerializeField]
        Transform target;

        [HTitle("Option")]
        [SerializeField]
        bool useAnimation = false;
        [SerializeField]
        float animationDuration = 0.2f;

        [HTitle("Scales")]
        [Tooltip("MUST consider the pivot relation with parent.")]
        public bool UseAbsoluteScale = false;
        [SerializeField]
        Vector2 originalScale;
        [SerializeField]
        Vector2 absoluteScale = Vector2.zero;
        [SerializeField]
        float scaleFactor = 1f;
#endif


        private void _Init() {
            originalScale = target.localScale;
        }

        public void Reset() => _Scale(originalScale);
        public void ChangeScale() => _Scale(UseAbsoluteScale ? absoluteScale : target.localScale * scaleFactor);


        private void _Scale(Vector2 scale) {
            if (useAnimation) {
                target.DOKill();
                target.DOScale(scale, animationDuration);
            }
            else {
                target.localScale = scale;
            }
        }
    }
}