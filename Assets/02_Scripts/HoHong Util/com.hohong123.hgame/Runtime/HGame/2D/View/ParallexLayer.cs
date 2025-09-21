#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� �з����� ���̾ƿ� �ý����� ������ ��ũ��Ʈ�Դϴ�.
 * 
 * ** ���� **
 * 1. 'userParallaxFollow'�� ī�޶� ��ġ�� ���� ���̾ƿ��� ���ݾ� ������ �������� �����̰ų� �������� ������ �� �ֽ��ϴ�.
 * + 'parallaxFactor'�� ���� �����̴� �ӵ��� ������ �� �ֽ��ϴ�. (0 = ����, 1 = ī�޶�� ������ �ӵ�)
 * 2. 'randomGap'���� �ʱ�ȭ �������� �� Ÿ�Ͽ� ���� ���� ������ ������ �� �ֽ��ϴ�.
 * 3. 'tiles'�� �ݵ�� 2�� �̻��� �з������� ���� �������� �Ҵ��մϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using HUtil.Collection;
using HUtil.Inspector;

namespace HGame._2D.View {
    public partial class ParallexLayer : MonoBehaviour {
#if !ODIN_INSPECTOR
        [HTitle("Camera")]
        [SerializeField]
        Camera cam;

        Transform camTran;

        [HTitle("Motion")]
        [SerializeField]
        bool useParallaxFollow = true;
        [SerializeField]
        [Range(0f, 1f)]
        float parallaxFactor = 0.2f;

        Vector3 preCamPos;

        [HTitle("Tile")]
        [SerializeField]
        bool randomGap = false;
        [SerializeField]
        float tileGap = 0f;
        [SerializeField]
        [Tooltip("X = min, Y = max")]
        Vector2 randomRange = new Vector2(0, 50);
        [SerializeField]
        SpriteRenderer[] tiles;
        [SerializeField]
        Vector2 tileWorldSize;
#endif

        int leftCount = 0;
        int rightCount = 0;
        float tileWidth = 0f;
        float[] multiTileWidth;
        CircularList<Transform> tileTrans;

        private float _GetTileWith => (randomGap ? multiTileWidth[tileTrans.Pivot] : tileWidth);


        #region Unity Life Cycle
        private void Start() {
            camTran = cam.transform;
            if (useParallaxFollow) {
                preCamPos = camTran.position;
            }

            tileWorldSize = tiles[0].bounds.size;
            tileWidth = tileWorldSize.x;

            if (randomGap) {
                int size = tiles.Length;
                float min = Mathf.Min(randomRange.x, randomRange.y);
                float max = Mathf.Max(randomRange.x, randomRange.y);
                multiTileWidth = new float[size];
                for (int k = 0; k < size; k++)
                    multiTileWidth[k] = tileWidth + Random.Range(min, max);
            }
            else {
                tileWidth += tileGap;
            }

            bool isOdd = tiles.Length % 2 != 0;
            tileTrans = new(pivot: tiles.Length / 2 - (isOdd ? 0 : 1), size: tiles.Length);
            foreach (var tile in tiles) {
                tileTrans.Add(tile.transform);
            }
#if !UNITY_EDITOR
            tiles = null;
#endif

            leftCount = (tileTrans.Count - 1) / 2;
            rightCount = tileTrans.Count - 1 - leftCount;

            _RepositionLine(tileTrans.CurrentItem);
        }

        private void LateUpdate() {
            if (useParallaxFollow) {
                Vector3 camDelta = camTran.position - preCamPos;
                if (camDelta.sqrMagnitude > 0f) {
                    Vector3 move = new Vector3(camDelta.x * parallaxFactor, 0f, 0f);
                    for (int k = 0; k < tileTrans.Count; k++)
                        tileTrans.Items[k].position += move;
                }
                preCamPos = camTran.position;
            }

            var center = tileTrans.CurrentItem;
            float half = _GetTileWith * 0.5f;
            if (camTran.position.x - center.position.x > half) {
                _ShiftRight();
            }
            else if (center.position.x - camTran.position.x > half) {
                _ShiftLeft();
            }
        }
#endregion

        #region Init
        private void _RepositionLine(Transform center) {
            // Allocate left side (Closest first)
            for (int k = 1; k <= leftCount; k++) {
                var leftTile = tileTrans.Items[(tileTrans.Pivot - k + tileTrans.Count) % tileTrans.Count];
                leftTile.position = center.position + Vector3.left * (_GetTileWith * k);
            }
            // Allocate right side
            for (int k = 1; k <= rightCount; k++) {
                var rightTile = tileTrans.Items[(tileTrans.Pivot + k) % tileTrans.Count];
                rightTile.position = center.position + Vector3.right * (_GetTileWith * k);
            }
        }
        #endregion

        #region Shift
        private void _ShiftRight() {
            var left = tileTrans.PeekOffset(-leftCount);
            var right = tileTrans.PeekOffset(rightCount);
            left.position = right.position + Vector3.right * _GetTileWith;
            tileTrans.MoveNext();
        }

        private void _ShiftLeft() {
            var left = tileTrans.PeekOffset(-leftCount);
            var right = tileTrans.PeekOffset(rightCount);
            right.position = left.position + Vector3.left * _GetTileWith;
            tileTrans.MovePrev();
        }
        #endregion
    }
}

#if UNITY_EDITOR
/* @Jason - PKH 05.09.25
 * 1. ��ƿ ��Ű���� ��ȯ ����Ʈ�� ����Ͽ� �з����� ����� �����մϴ�.
 * 2. ������ �з����� �̹��� ���ҽ��� ������ ���� ¦�� Ȥ�� Ȧ�� ���Ÿ���� ������ �� �ֽ��ϴ�.
 * + ¦���� ���, (�߰� - 1)�� �̹����� ����(����) �̹����� �νĵ˴ϴ�. 
 * + ������ ��ġ�� �ش� �з������� �����ϴ� ī�޶��� �߽����� �����Ͻð� �¿쿡 �� �̹������� ��ġ�Ͻø� �˴ϴ�.
 * ++ Ex) 2���� �̹��� = 1��°�� ����
 * ++ Ex) 4���� �̹��� = 2��°�� ����
 * ++ Ex) 5���� �̹��� = 3��°�� ����
 * ============================================================
 * @Jason - PKH 06.09.25
 * TODO :: Shift ������ ��ɵ��� ���� ���ȭ�� ����.
 */
#endif