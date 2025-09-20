#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2Dȯ�� 'MapManager' �̴ϸ� ��ɿ� �̴ϸ� ������ �����ϱ� ���� ������Ʈ ��ũ��Ʈ�Դϴ�.
 * SpriteRenderer�� ũ��� ������ �����մϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using Util.OdinCompat;

namespace HGame._2D.Map {
    [DisallowMultipleComponent]
    public class SpriteRendererBoundsSource : MonoBehaviour, IWorldBoundSource {
        [HeaderOrTitle("Boundary")]
        [SerializeField]
        SpriteRenderer spriteRender;

        private void Awake() {
            if (!spriteRender) spriteRender = GetComponentInChildren<SpriteRenderer>();
        }

        public bool TryGetWorldRect(out Rect rect) {
            rect = default;
            if (!spriteRender) return false;

            var b = spriteRender.bounds;
            rect = new Rect(b.min, b.size);

            return true;
        }
    }
}