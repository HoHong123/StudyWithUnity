using System;
using UnityEngine;
using DG.Tweening;
using HUtil.Inspector;


namespace HUtil.UI.Entity {
    [Serializable]
    public partial class MovingUiEntity {
#if !ODIN_INSPECTOR
        [HTitle("Target")]
        [SerializeField]
        Transform target;

        [HTitle("Option")]
        [SerializeField]
        bool useAnimation = false;
        [SerializeField]
        float animationDuration = 0.2f;

        [HTitle("Positions")]
        [Tooltip("MUST consider the pivot relation with parent.")]
        public bool UseAbsolutePosition = false;
        [SerializeField]
        Vector3 originPosition;
        [SerializeField]
        Vector3 absolutePosition = Vector3.zero;
        [SerializeField]
        Vector3 moveAmount = Vector3.zero;
#endif

        private void _Init() {
            originPosition = target.localPosition;
        }


        public void Reset() => _Move(originPosition);
        public void Move() => _Move(UseAbsolutePosition ? absolutePosition : (target.localPosition + moveAmount));


        private void _Move(Vector3 pos) {
            if (useAnimation) {
                target.DOKill();
                target.DOLocalMove(pos, animationDuration);
            }
            else {
                target.localPosition = pos;
            }
        }
    }

}