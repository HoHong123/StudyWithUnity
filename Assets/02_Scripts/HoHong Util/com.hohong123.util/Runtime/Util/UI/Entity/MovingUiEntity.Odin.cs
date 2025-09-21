#if ODIN_INSPECTOR
using UnityEngine;
using Sirenix.OdinInspector;
using HUtil.Inspector;
#endif

namespace HUtil.UI.Entity {
    public partial class MovingUiEntity {
#if ODIN_INSPECTOR
        [HTitle("Target")]
        [SerializeField]
        [OnValueChanged(nameof(_Init))]
        Transform target;

        [HTitle("Option")]
        [SerializeField]
        bool useAnimation = false;
        [ShowIf(nameof(useAnimation)), SerializeField]
        float animationDuration = 0.2f;

        [HTitle("Positions")]
        [InfoBox("MUST consider the pivot relation with parent.")]
        public bool UseAbsolutePosition = false;
        [SerializeField]
        Vector3 originPosition;
        [ShowIf("UseAbsolutePosition")]
        [SerializeField]
        Vector3 absolutePosition = Vector3.zero;
        [HideIf("UseAbsolutePosition")]
        [SerializeField]
        Vector3 moveAmount = Vector3.zero;
#endif
    }
}