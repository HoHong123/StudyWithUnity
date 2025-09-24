#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HTitle �Ӽ� ��ũ��Ʈ�Դϴ�.
 * Header����� ��ü�ϱ� ���� �ۼ��Ǿ����ϴ�.
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

            // �ʱ� UI ����ȭ
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
