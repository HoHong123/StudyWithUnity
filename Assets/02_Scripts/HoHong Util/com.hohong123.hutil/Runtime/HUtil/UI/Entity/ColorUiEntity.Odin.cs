using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using HUtil.Inspector;

namespace HUtil.UI.Entity {
    [Serializable]
    public partial class ColorUiEntity {
#if ODIN_INSPECTOR
        [HTitle("Option")]
        [SerializeField]
        bool changeSprite = false;
        [HideIf(nameof(changeSprite)), SerializeField]
        bool useAnimation = false;
        [ShowIf("@!this.changeSprite && this.useAnimation"), SerializeField]
        float animationDuration = 0.2f;

        [HTitle("Color")]
        [OnValueChanged(nameof(_Init))]
        [HideIf(nameof(changeSprite))]
        [SerializeField]
        MaskableGraphic graphic;
        [HideIf(nameof(changeSprite))]
        [SerializeField]
        Color originColor;
        [HideIf(nameof(changeSprite))]
        [SerializeField]
        Color targetColor;

        [HTitle("Sprite")]
        [OnValueChanged(nameof(_Init))]
        [ShowIf(nameof(changeSprite))]
        [SerializeField]
        Image image;
        [ShowIf(nameof(changeSprite)), SerializeField]
        Sprite originSprite;
        [ShowIf(nameof(changeSprite)), SerializeField]
        Sprite targetSprite;
#endif
    }
}