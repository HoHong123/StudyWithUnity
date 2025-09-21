#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� �̴ϸ��� ����ϴ� ��ũ��Ʈ�Դϴ�.
 * 
 * ** ���� **
 * 1. 'MapBoundType'���� �̴ϸ� ������ �����ϴ� ����� ������ �� �ֽ��ϴ�. (�ڼ��� ������ �ش� ������ ���� Ȯ��)
 * 2. �ʼ� ����� �ν����Ϳ��� �ݵ�� �־���մϴ�.
 * 
 * ** ��� ���� **
 * ---- �ʼ� ��� ----
 * + cam : �̴ϸʿ��� ���� ī�޶�
 * + boundType : �̴ϸ� ������ ������ ����� �����ϴ� ������
 * ++ worldBoundsB2D : Box2D �ݶ��̴��� �̴ϸ� ���� ����
 * ++ worldBoundSources : 'IWorldBoundSource'�� ����� ��ũ��Ʈ�� �̴ϸ� ���� ����
 * ++ absolutBound : Rect ������ �̴ϸ� ���� ����
 * + camArea : �̴ϸʿ� ǥ���� ī�޶� �ٷκ��� ������ ����� �̹��� UI ������Ʈ RectTransform
 * + mapArea : �̴ϸ����� ���� �̹��� UI ������Ʈ RectTransform
 * + markerPrefab : �̴ϸʿ� ǥ���� ������Ʈ���� ��Ÿ�� �⺻ �̹��� UI ������Ʈ
 * + defaultMarkerSpt : ����Ǵ� ��Ŀ ������Ʈ�� Ŀ���� Sprite �̹����� �������� �ʴ´ٸ� ������ �⺻ Sprite �̹���
 * ---- �ɼ� ��� ----
 * + mapPanel : �̴ϸ� �̹����� ��ȣ�ۿ�(Ŭ��, �巡��)�� ����ϴ� �г� ��ũ��Ʈ
 * + markerParent : ��Ŀ ������Ʈ Ǯ���� ���Ǵ� �θ� ��ü
 * + autoFitMinimapAspect : 'mapArea' �̹��� ������ 'boundType'�� ������ �� ������ ���ߴ� �ɼ�
 * + fitPadding : ���� ���� �е� ��
 * =========================================================
 */
#endif

#if ODIN_INSPECTOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using HUtil.Core;
using HUtil.UI.Panel;
using HUtil.Inspector;
#endif

namespace HGame._2D.Map {
    public partial class MapManager : SingletonBehaviour<MapManager> {
#if ODIN_INSPECTOR
        [HTitle("Camera")]
        [SerializeField]
        Camera cam;

        [HTitle("Bounds")]
        [SerializeField]
        MapBoundType boundType;
        [SerializeField]
        float worldZ = -10f;
        [SerializeField, ShowIf("boundType", MapBoundType.WorldBox)]
        BoxCollider2D worldBoundsB2D;
        [SerializeField, ShowIf("boundType", MapBoundType.BoundSource)]
        List<MonoBehaviour> worldBoundSources = new();
        [SerializeField, ShowIf("boundType", MapBoundType.Absolute)]
        Rect absolutBound;

        [HTitle("UI")]
        [SerializeField]
        RectTransform camArea;
        [SerializeField]
        RectTransform mapArea;
        [SerializeField]
        ProxyPanel mapPanel;

        [HTitle("Marker")]
        [SerializeField]
        Image markerPrefab;
        [SerializeField]
        Sprite defaultMarkerSpt;
        [SerializeField, Tooltip("Must be a child of map")]
        Transform markerParent;

        [HTitle("Minimap Auto Fit")]
        [SerializeField]
        bool autoFitMinimapAspect = true;
        [SerializeField, ShowIf("autoFitMinimapAspect")]
        Vector2 fitPadding = new Vector2(8, 8);

        [HTitle("Options")]
        [SerializeField]
        bool isYAxisUp = true;
        [SerializeField]
        bool allowDragNavigate = true;
#endif
    }
}
