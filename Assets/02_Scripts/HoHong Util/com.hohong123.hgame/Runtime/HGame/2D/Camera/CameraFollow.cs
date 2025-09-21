#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� Ư�� ������Ʈ�� ����ٴϴ� ī�޶� ����Ʈ ��ũ��Ʈ�Դϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using HUtil.Inspector;
using HGame._2D.Map;

namespace HGame._2D.Cam {
    [DisallowMultipleComponent]
    public partial class CameraFollow : MonoBehaviour {
#if !ODIN_INSPECTOR
        [HTitle("Camear")]
        [SerializeField]
        Camera cam;
        [SerializeField, Range(0f, 1f)]
        float smooth = 0.15f;
        [SerializeField]
        float zPos = -10f;

        [HTitle("Target")]
        [SerializeField]
        Transform target;
        [SerializeField]
        Transform originalTarget;

        [HTitle("Bounds")]
        [SerializeField]
        MapBoundType boundType;
        [SerializeField]
        BoxCollider2D worldBoundsB2D;
        [SerializeField]
        Rect absolutBound;

#if UNITY_EDITOR
        [HTitle("Debug")]
        [SerializeField]
        protected bool useDebug = true;
        [SerializeField]
        protected Color debugColor = Color.cyan;
#endif
#endif

        bool hasRect = false;
        Rect worldRect = default;
        Vector3 velocity = default;


        private void Awake() {
            _RefreshWorldRect();
        }

        private void LateUpdate() {
            if (!cam || !hasRect) return;

            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            float minX = worldRect.xMin + halfW;
            float maxX = worldRect.xMax - halfW;
            float minY = worldRect.yMin + halfH;
            float maxY = worldRect.yMax - halfH;

            Vector3 desired = target.position;
            float newX = Mathf.Clamp(desired.x, minX, maxX);
            float newY = Mathf.Clamp(desired.y, minY, maxY);

            Vector3 dest = new Vector3(newX, newY, zPos);
            cam.transform.position = (smooth <= 0f)  ? dest : Vector3.SmoothDamp(cam.transform.position, dest, ref velocity, smooth);
        }


        public void ResetTarget() => target = originalTarget;
        public void SetPosition(Vector3 position) => target.position = position;
        public void SetPosition(Transform target) => this.target = target;


        private void _RefreshWorldRect() {
            hasRect = true;
            switch (boundType) {
            case MapBoundType.WorldBox:
                if (worldBoundsB2D) {
                    var b = worldBoundsB2D.bounds;
                    worldRect = new Rect(b.min, b.size);
                }
                break;
            case MapBoundType.Absolute:
                if (absolutBound.size != Vector2.zero) {
                    worldRect = absolutBound;
                }
                break;
            default:
                hasRect = false;
                break;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            if (!useDebug) return;
            switch (boundType) {
            case MapBoundType.WorldBox:
                if (worldBoundsB2D) {
                    var b = worldBoundsB2D.bounds;
                    Gizmos.color = debugColor;
                    Gizmos.DrawWireCube(b.center, b.size);
                }
                break;
            case MapBoundType.Absolute:
                if (absolutBound.size != Vector2.zero) {
                    Gizmos.color = debugColor;
                    Gizmos.DrawWireCube((Vector3)absolutBound.center, (Vector3)absolutBound.size);
                }
                break;
            default:
                hasRect = false;
                break;
            }
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
 */
#endif