using UnityEngine;
using Sirenix.OdinInspector;
using HUtil.Inspector;


namespace HUtil.UI.Entity {
    public partial class ScalingUiEntity {
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

        [HTitle("Scales")]
        [Tooltip("MUST consider the pivot relation with parent.")]
        public bool UseAbsoluteScale = false;
        [SerializeField]
        Vector2 originalScale;
        [ShowIf("UseAbsoluteScale")]
        [SerializeField]
        Vector2 absoluteScale = Vector2.zero;
        [HideIf("UseAbsoluteScale")]
        [SerializeField]
        float scaleFactor = 1f;
#endif
    }
}