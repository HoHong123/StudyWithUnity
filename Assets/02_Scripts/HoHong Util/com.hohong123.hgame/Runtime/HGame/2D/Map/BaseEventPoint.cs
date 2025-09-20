#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� Ư�� ������Ʈ�� �浹 �̺�Ʈ�� ����ϴ� ��ũ��Ʈ�� �����߽��ϴ�.
 * 
 * ** ���� **
 * 1. ������� ���׸� Ÿ�Կ� �浹 �� ������ ������Ʈ�� �����մϴ�.
 * 2. ���� �浹�� �߻��ϸ� �浹 ������Ʈ�� �±׸� ���մϴ�.
 * + �±� ������� �������� ('useTag' ����)
 * 3. �浹 �� ������ ������Ʈ�� �������� �븮�ڸ� ���� ����� ��� ������Ʈ�� �����մϴ�.
 * 4. �����ͻ� �̺�Ʈ ����Ʈ�� ����� Ȱ��ȭ�� �� �ֽ��ϴ�.
 * =========================================================
 */
#endif

using System;
using UnityEngine;
using Util.Logger;
using Util.OdinCompat;

namespace HGame._2D.Map {
    public abstract partial class BaseEventPoint<T> : MonoBehaviour where T : MonoBehaviour {
#if !ODIN_INSPECTOR
        [HeaderOrTitle("Filter")]
        [SerializeField]
        protected bool useTag = true;
        [SerializeField]
        protected string targetTag;

        [HeaderOrTitle("Collider")]
        [SerializeField]
        protected Collider2D eventCollider;

#if UNITY_EDITOR
        [HeaderOrTitle("Debug")]
        [SerializeField]
        protected bool useDebug = true;
        [SerializeField]
        protected Color debugColor = Color.red;
#endif
#endif

        public string TargetTag => targetTag;

        public event Action<T> OnEventCollision;
        public event Action<T> OnEventTrigger;


        protected virtual void OnCollisionEnter2D(Collision2D collision) {
            if (useTag && !collision.transform.CompareTag(TargetTag)) return;
            if (!collision.transform.TryGetComponent(out T target)) return;
            OnEventCollision?.Invoke(target);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) {
            if (useTag && !collision.CompareTag(TargetTag)) return;
            if (!collision.transform.TryGetComponent(out T target)) return;
            OnEventTrigger?.Invoke(target);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if (!eventCollider) TryGetComponent(out eventCollider);
            if (!string.IsNullOrEmpty(targetTag)) {
                var all = UnityEditorInternal.InternalEditorUtility.tags;
                bool ok = Array.IndexOf(all, targetTag) >= 0;
                if (!ok) {
                    HLogger.Error($"{name}: '{targetTag}' �±װ� Tag Manager�� �����ϴ�.", gameObject);
                    targetTag = string.Empty;
                }
            }
        }

        private void OnDrawGizmosSelected() {
            if (!useDebug) return;
            Gizmos.color = debugColor;
            Gizmos.DrawCube(eventCollider.bounds.center, eventCollider.bounds.size);
        }
#endif
    }
}

#if UNITY_EDITOR
/* Dev Log
 * @Jason - PKH 18. 09. 25
 * 1. ����� �ý��� ������Ʈ
 * + ����� Ȱ��ȭ ��� �߰�
 * + ����� �÷� ���� ����
 * 2. �±� ��뼳�� �߰�
 */
#endif