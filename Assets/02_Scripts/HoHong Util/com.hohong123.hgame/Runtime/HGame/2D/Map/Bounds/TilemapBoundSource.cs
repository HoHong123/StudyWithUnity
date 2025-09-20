#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2Dȯ�� 'MapManager' �̴ϸ� ��ɿ� �̴ϸ� ������ �����ϱ� ���� ������Ʈ ��ũ��Ʈ�Դϴ�.
 * Tilemap�� ũ��� ������ �����մϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using UnityEngine.Tilemaps;
using Util.OdinCompat;

namespace HGame._2D.Map {
    [DisallowMultipleComponent]
    public class TilemapBoundSource : MonoBehaviour, IWorldBoundSource {
        [HeaderOrTitle("Boundary")]
        [SerializeField]
        Tilemap tilemap;

        private void Awake() {
            if (!tilemap) tilemap = GetComponentInChildren<Tilemap>();
        }

        public bool TryGetWorldRect(out Rect rect) {
            rect = default;
            if (!tilemap) return false;

            var cell = tilemap.cellBounds; // Grid space
            var min = tilemap.CellToWorld(cell.min);
            var max = tilemap.CellToWorld(cell.max);
            var size = (Vector2)(max - min);
            rect = new Rect(min, size);

            return size.x > 0 && size.y > 0;
        }
    }
}
