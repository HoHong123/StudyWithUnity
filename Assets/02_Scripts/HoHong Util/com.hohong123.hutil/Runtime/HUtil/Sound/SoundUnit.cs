using UnityEngine;
using HUtil.Inspector;

namespace HUtil.Sound {
    public class SoundUnit : MonoBehaviour {
        [HTitle("Container")]
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty, Sirenix.OdinInspector.HideLabel]
#endif
        SoundContainer container;


        private void Awake() {
            SoundManager.Instance.SetSoundUnit(container);
        }

        private void OnDestroy() {
            if (Application.isPlaying && SoundManager.HasInstance)
                SoundManager.Instance.RemoveSoundUnit(container);
        }
    }
}