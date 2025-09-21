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

using Sirenix.OdinInspector;
using UnityEngine;
using HUtil.Inspector;

namespace HGame._2D.View {
    public partial class ParallexLayer : MonoBehaviour {
#if ODIN_INSPECTOR
        [HTitle("Camera")]
        [SerializeField]
        Camera cam;

        Transform camTran;

        [HTitle("Motion")]
        [SerializeField]
        bool useParallaxFollow = true;
        [SerializeField, ShowIf("useParallaxFollow")]
        [Range(0f, 1f)]
        float parallaxFactor = 0.2f;

        Vector3 preCamPos;

        [HTitle("Tile")]
        [SerializeField]
        bool randomGap = false;
        [SerializeField, HideIf("randomGap")]
        float tileGap = 0f;
        [SerializeField, ShowIf("randomGap")]
        [Tooltip("X = min, Y = max")]
        Vector2 randomRange = new Vector2(0, 50);
        [SerializeField]
        SpriteRenderer[] tiles;
        [SerializeField, ReadOnly]
        Vector2 tileWorldSize;
#endif
    }
}