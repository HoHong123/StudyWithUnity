using UnityEngine;

namespace Study.AS.Mvc {
    public sealed class MvcController : MonoBehaviour {
        [SerializeField] 
        MvcView view;

        readonly MvcModel model = new();


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
