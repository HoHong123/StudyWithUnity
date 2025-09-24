#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HTitle 속성 스크립트입니다.
 * Header기능을 대체하기 위해 작성되었습니다.
 * =========================================================
 */
#endif
using UnityEngine;
using HUtil.Inspector;

namespace Study.AS.Mvc {
    public sealed class MvcController : MonoBehaviour {
        [HTitle("View")]
        [SerializeField]
        MvcView view;

        [HTitle("Model")]
        [SerializeField]
        MvcModel model = new();


        private void OnEnable() {
            model.Changed += _OnModelChanged;
            view.OnClickIncrement += _OnIncrementClicked;

            // 초기 UI 동기화
            view.SetValue(model.Value);
        }

        private void OnDisable() {
            model.Changed -= _OnModelChanged;
            view.OnClickIncrement -= _OnIncrementClicked;
        }

        private void _OnIncrementClicked() => model.Increment(1);

        private void _OnModelChanged(int value) => view.SetValue(value);
    }
}
