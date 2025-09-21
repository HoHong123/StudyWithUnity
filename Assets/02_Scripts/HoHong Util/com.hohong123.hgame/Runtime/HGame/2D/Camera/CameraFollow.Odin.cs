#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� Ư�� ������Ʈ�� ����ٴϴ� ī�޶� ����Ʈ ��ũ��Ʈ�Դϴ�.
 * =========================================================
 */
#endif

#if ODIN_INSPECTOR
using HGame._2D.Map;
using Sirenix.OdinInspector;
using UnityEngine;
using HUtil.Inspector;
#endif

namespace HGame._2D.Cam {
    public partial class CameraFollow : MonoBehaviour {
#if ODIN_INSPECTOR
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
        [SerializeField, Required]
        Transform originalTarget;

        [HTitle("Bounds")]
        [SerializeField]
        MapBoundType boundType;
        [SerializeField, ShowIf("boundType", MapBoundType.WorldBox)]
        BoxCollider2D worldBoundsB2D;
        [SerializeField, ShowIf("boundType", MapBoundType.Absolute)]
        Rect absolutBound;

#if UNITY_EDITOR
        [HTitle("Debug")]
        [SerializeField]
        protected bool useDebug = true;
        [SerializeField, ShowIf("useDebug")]
        protected Color debugColor = Color.cyan;
#endif
#endif
    }
}