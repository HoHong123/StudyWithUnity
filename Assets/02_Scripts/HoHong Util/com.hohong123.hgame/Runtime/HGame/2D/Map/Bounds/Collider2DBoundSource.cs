#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2Dȯ�� 'MapManager' �̴ϸ� ��ɿ� �̴ϸ� ������ �����ϱ� ���� ������Ʈ ��ũ��Ʈ�Դϴ�.
 * Collider2D ������Ʈ�� �ٿ������ Ȯ���Ͽ� ������ �����մϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using Util.OdinCompat;

namespace HGame._2D.Map {
    [DisallowMultipleComponent]
    public class Collider2DBoundSource : MonoBehaviour, IWorldBoundSource {
        [HeaderOrTitle("Boundary")]
        [SerializeField]
        Collider2D coll2D;

        private void Awake() {
            if (!coll2D) coll2D = GetComponentInChildren<Collider2D>();
        }

        public bool TryGetWorldRect(out Rect rect) {
            rect = default;
            if (!coll2D) return false;

            var bound = coll2D.bounds;
            rect = new Rect(bound.min, bound.size);

            return true;
        }
    }
}