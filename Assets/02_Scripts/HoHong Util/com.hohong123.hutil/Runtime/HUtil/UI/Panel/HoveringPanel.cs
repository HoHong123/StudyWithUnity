#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �����Ͱ� UI ���� ȣ���� ���¶�� �����ϴ� �г� ��� ��ũ��Ʈ�Դϴ�.
 * 
 * ** ����� **
 * 1. �ش� ��ũ��Ʈ�� ȣ���� ����� �ʿ��� UI�� ������Ʈ�� ����մϴ�.
 * 2. �ش� ������Ʈ�� ����� ��Ʈ�ѷ��� '�̺�Ʈ ����/���/����'�� ����� �̺�Ʈ�� �븮�ڿ� ����մϴ�.
 * =========================================================
 */
#endif

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HUtil.UI.Panel {
    public class HoveringPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        CancellationTokenSource hoverCts;

        public float Duration { get; set; } = 2f;

        public event Action OnPointerEnterEvent;
        public event Action OnPointerExitEvent;
        public event Action<Vector2> OnHoveringComplete;


        private void OnEnable() {
            OnPointerEnterEvent += _StartHoverCheck;
            OnPointerExitEvent += _StopHoverCheck;
        }

        private void OnDisable() {
            OnPointerEnterEvent -= _StartHoverCheck;
            OnPointerExitEvent -= _StopHoverCheck;
            _StopHoverCheck();
        }


        public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterEvent?.Invoke();
        public void OnPointerExit(PointerEventData eventData) => OnPointerExitEvent?.Invoke();


        private void _StartHoverCheck() {
            _StopHoverCheck();
            hoverCts = new CancellationTokenSource();
            _HoverAsync(hoverCts.Token).Forget();
        }

        private void _StopHoverCheck() {
            if (hoverCts == null) return;
            hoverCts.Cancel();
            hoverCts.Dispose();
            hoverCts = null;
        }

        private async UniTaskVoid _HoverAsync(CancellationToken ct) {
            try {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(Duration),
                    DelayType.DeltaTime,
                    PlayerLoopTiming.Update,
                    ct);

                if (!ct.IsCancellationRequested)
                    OnHoveringComplete?.Invoke((Vector2)transform.position);
            }
            catch (OperationCanceledException) {
                // Cancel
            }
        }
    }
}
