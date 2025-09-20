#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * Ư�� UI�� ���� ��ȣ�ۿ��� �ʿ�������, �Ź� ��Ʈ�ѷ��� ���� �����ϱ� ���ŷӱ⿡ �ۼ��� ��ũ��Ʈ�Դϴ�.
 * 
 * ** ����� **
 * 1. ��Ī �״�� ���Ͻ� ������ �ϸ�, ��ȣ�ۿ��� �ʿ��� UI�� ������Ʈ�� �ֽ��ϴ�.
 * 2. ���ϴ� �̺�Ʈ�� ������ �Լ����� �븮�ڿ� ����մϴ�.
 * 3. �ߺ� �巡�� ���� ����� �ʿ��ϴٸ� 'SetAutoDragCheck'�Լ��� �ش� ���Ͻ� ��ũ��Ʈ�� ����ϸ� �˴ϴ�.
 * =========================================================
 */
#endif

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util.UI.Panel {
    public class ProxyPanel : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
        IPointerClickHandler, IPointerUpHandler, IPointerDownHandler,
        IDropHandler {

        public event Action<PointerEventData> BeginDragEvent;
        public event Action<PointerEventData> EndDragEvent;
        public event Action<PointerEventData> OnDragEvent;
        public event Action<PointerEventData> OnDropEvent;
        public event Action<PointerEventData> PointerEnterEvent;
        public event Action<PointerEventData> PointerExitEvent;
        public event Action<PointerEventData> PointerMoveEvent;
        public event Action<PointerEventData> PointerClickEvent;
        public event Action<PointerEventData> PointerUpEvent;
        public event Action<PointerEventData> PointerDownEvent;

        public void SetAutoDragCheck(object proxyOwner) {
            BeginDragEvent += (eventData) => UiEvent.LockDrag(proxyOwner);
            EndDragEvent += (eventData) => UiEvent.UnlockDrag(proxyOwner);
        }

        public void OnBeginDrag(PointerEventData eventData) => BeginDragEvent?.Invoke(eventData);
        public void OnEndDrag(PointerEventData eventData) => EndDragEvent?.Invoke(eventData);
        public void OnDrag(PointerEventData eventData) => OnDragEvent?.Invoke(eventData);
        public void OnDrop(PointerEventData eventData) => OnDropEvent?.Invoke(eventData);
        public void OnPointerEnter(PointerEventData eventData) => PointerEnterEvent?.Invoke(eventData);
        public void OnPointerExit(PointerEventData eventData) => PointerExitEvent?.Invoke(eventData);
        public void OnPointerMove(PointerEventData eventData) => PointerMoveEvent?.Invoke(eventData);
        public void OnPointerClick(PointerEventData eventData) => PointerClickEvent?.Invoke(eventData);
        public void OnPointerUp(PointerEventData eventData) => PointerUpEvent?.Invoke(eventData);
        public void OnPointerDown(PointerEventData eventData) => PointerDownEvent?.Invoke(eventData);
    }
}
