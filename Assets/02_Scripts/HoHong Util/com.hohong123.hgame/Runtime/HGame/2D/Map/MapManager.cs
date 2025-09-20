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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util.Core;
using Util.Pooling;
using Util.UI.Panel;
using Util.OdinCompat;
using HGame._2D.Cam;

namespace HGame._2D.Map {
    [DisallowMultipleComponent]
    public partial class MapManager : SingletonBehaviour<MapManager> {
#if !ODIN_INSPECTOR
        [HeaderOrTitle("Camera")]
        [SerializeField]
        Camera cam;

        [HeaderOrTitle("Bounds")]
        [SerializeField]
        MapBoundType boundType;
        [SerializeField]
        float worldZ = -10f;
        [SerializeField]
        BoxCollider2D worldBoundsB2D;
        [SerializeField]
        List<MonoBehaviour> worldBoundSources = new();
        [SerializeField]
        Rect absolutBound;

        [HeaderOrTitle("UI")]
        [SerializeField]
        RectTransform camArea;
        [SerializeField]
        RectTransform mapArea;
        [SerializeField]
        ProxyPanel mapPanel;

        [HeaderOrTitle("Marker")]
        [SerializeField]
        Image markerPrefab;
        [SerializeField]
        Sprite defaultMarkerSpt;
        [SerializeField, Tooltip("Must be a child of map")]
        Transform markerParent;

        [HeaderOrTitle("Minimap Auto Fit")]
        [SerializeField]
        bool autoFitMinimapAspect = true;
        [SerializeField]
        Vector2 fitPadding = new Vector2(8, 8);
        [SerializeField]
        Vector2 fitPadding = new Vector2(8, 8);

        [HeaderOrTitle("Options")]
        [SerializeField]
        bool isYAxisUp = true;
        [SerializeField]
        bool allowDragNavigate = true;
#endif

        bool dragging;
        bool hasWorldRect;
        Rect cachedWorldRect;
        ComponentPool<Image> markerPool;
        readonly Dictionary<MinimapTrackable, Image> trackables = new();


        #region Unity Life-Cycle
        private void Start() {
            markerPool = new(
                markerPrefab,
                5,
                markerParent,
                onCreate: (marker) => marker.gameObject.SetActive(false),
                onGet: (marker) => marker.gameObject.SetActive(true),
                onReturn: (marker) => marker.gameObject.SetActive(false)
                );

            if (!mapPanel) {
                mapPanel.PointerClickEvent += _OnPointerClick;
                mapPanel.BeginDragEvent += _OnBeginDrag;
                mapPanel.OnDragEvent += _OnDrag;
                mapPanel.EndDragEvent += _OnEndDrag;
            }
        }

        private void OnEnable() {
            _RefreshWorldRect();
            if (autoFitMinimapAspect) {
                _FitMinimapRectToWorldAspect();
            }
        }

        private void LateUpdate() {
            // Update tracker icon
            foreach (var track in trackables.Keys) {
                _UpdateIconPosition(track);
                if (trackables[track].IsActive() && track.ScaleByCollider)
                    _UpdateIconScaleByCollider(track);
            }
            // Update camera view and marker
            if (camArea && cam && cam.orthographic) {
                _UpdateCameraViewMarker();
            }
        }
        #endregion

        #region Register
        public void Register(MinimapTrackable track) {
            if (track == null) return;
            if (!trackables.ContainsKey(track)) {
                var img = markerPool.Get();
                img.sprite = (track.UseIcon) ? track.Icon : defaultMarkerSpt;
                trackables.Add(track, img);
            }
            _UpdateIconPosition(track);
        }

        public void Unregister(MinimapTrackable track) {
            if (track == null) return;
            markerPool.Return(trackables[track]);
            trackables.Remove(track);
        }
        #endregion

        #region World Calculation
        bool _IsInsideWorldRect(Vector2 worldPos) {
            return _GetWorldRect().Contains(worldPos);
        }

        private Rect _GetWorldRect() {
            if (!hasWorldRect) _RefreshWorldRect();
            return cachedWorldRect;
        }

        private Vector2 _ConvertWorldToMap(Vector2 worldPos) {
            var pivot = mapArea.pivot;
            var rect = mapArea.rect.size;
            var worldRect = _GetWorldRect();
            var newX = Mathf.InverseLerp(worldRect.xMin, worldRect.xMax, worldPos.x);
            var newY = Mathf.InverseLerp(worldRect.yMin, worldRect.yMax, worldPos.y);
            if (!isYAxisUp) newY = 1f - newY;
            return new Vector2((newX - pivot.x) * rect.x, (newY - pivot.y) * rect.y);
        }

        private Vector2 _ConvertMapToWorld(Vector2 localMiniPos) {
            var rect = mapArea.rect.size;
            var pivot = mapArea.pivot;
            var worldRect = _GetWorldRect();
            var newX = (localMiniPos.x / rect.x) + pivot.x;
            var newY = (localMiniPos.y / rect.y) + pivot.y;
            if (!isYAxisUp) newY = 1f - newY;
            var worldX = Mathf.Lerp(worldRect.xMin, worldRect.xMax, newX);
            var worldY = Mathf.Lerp(worldRect.yMin, worldRect.yMax, newY);
            return new Vector2(worldX, worldY);
        }
        #endregion

        #region Update Map
        private void _UpdateIconPosition(MinimapTrackable track) {
            if (!trackables.ContainsKey(track)) return;
            var marker = trackables[track];
            bool visible = track.ShowWhenOutOfBounds;
            if (!track.ShowWhenOutOfBounds) {
                visible = _IsInsideWorldRect(track.Target.position);
            }

            marker.gameObject.SetActive(visible);
            if (!visible) return;

            marker.rectTransform.anchoredPosition = _ConvertWorldToMap(track.Target.position);
        }

        private void _UpdateIconScaleByCollider(MinimapTrackable track) {
            if (!trackables.ContainsKey(track)) return;

            var col = track.Collider;
            Vector2 size = Vector2.one;
            if (col != null) {
                size = track.Collider.bounds.size;
            }
            else {
                var sr = track.gameObject.GetComponent<SpriteRenderer>();
                if (sr != null) size = sr.bounds.size;
            }

            // Approximate world size to minimap size scale (���� ũ�⸦ �̴ϸ� ũ�� �����Ϸ� �ٻ�)
            var wr = _GetWorldRect();
            var rect = mapArea.rect.size;
            var marker = trackables[track];
            float sizeX = Mathf.Clamp01(size.x / wr.width);
            float sizeY = Mathf.Clamp01(size.y / wr.height);
            float sizeN = Mathf.Clamp01(Mathf.Sqrt(sizeX * sizeY));
            float fix = Mathf.Lerp(track.IconSizeMin, track.IconSizeMax, sizeN);
            marker.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fix);
            marker.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fix);
        }

        private void _UpdateCameraViewMarker() {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            // ī�޶� �߽� => �̴ϸ� ��ǥ
            Vector2 miniCenter = _ConvertWorldToMap(cam.transform.position);
            // ī�޶� ����Ʈ ũ�⸦ �̴ϸ� ���� �����̽� �ȼ��� �ٻ�
            var worldRect = _GetWorldRect();
            var rect = mapArea.rect.size;

            float viewW = Mathf.Clamp01((halfW * 2f) / worldRect.width) * rect.x;
            float viewH = Mathf.Clamp01((halfH * 2f) / worldRect.height) * rect.y;

            camArea.anchoredPosition = miniCenter;
            camArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewW);
            camArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, viewH);
        }
        #endregion

        #region Camera
        private void _MoveCameraToWorld(Vector2 world) {
            var worldRect = _GetWorldRect();
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            float clampedX = Mathf.Clamp(world.x, worldRect.xMin + halfW, worldRect.xMax - halfW);
            float clampedY = Mathf.Clamp(world.y, worldRect.yMin + halfH, worldRect.yMax - halfH);

            var current = cam.transform.position;
            var dest = new Vector3(clampedX, clampedY, worldZ);

            CameraManager.Instance.ResetFollow();
            CameraManager.Instance.SetFollowTarget(dest);
        }
        #endregion

        #region Refresh
        private void _RefreshWorldRect() {
            hasWorldRect = true;

            switch (boundType) {
            case MapBoundType.WorldBox:
                if (worldBoundsB2D) {
                    var b = worldBoundsB2D.bounds;
                    cachedWorldRect = new Rect(b.min, b.size);
                }
                break;
            case MapBoundType.BoundSource:
                foreach (var bound in worldBoundSources) {
                    if (bound is IWorldBoundSource src && src.TryGetWorldRect(out var rect)) {
                        cachedWorldRect = rect;
                    }
                }
                break;
            case MapBoundType.Absolute:
                if (absolutBound.size != Vector2.zero) {
                    cachedWorldRect = absolutBound;
                }
                break;
            default:
                hasWorldRect = false;
                break;
            }
        }
        #endregion

        #region Fit Rect
        private void _FitMinimapRectToWorldAspect() {
            if (!mapArea || !hasWorldRect) return;

            var parent = mapArea.parent as RectTransform;
            if (!parent) return;

            var parentSize = parent.rect.size - fitPadding * 2f;
            var worldAspect = cachedWorldRect.width / Mathf.Max(0.0001f, cachedWorldRect.height);
            var parentAspect = parentSize.x / Mathf.Max(0.0001f, parentSize.y);

            Vector2 targetSize;
            // ���ΰ� �� �� ���, ���θ� ���߰� ���θ� ���δ�(���͹ڽ� ���Ʒ�)
            if (worldAspect > parentAspect) {
                float w = parentSize.x;
                float h = w / worldAspect;
                targetSize = new Vector2(w, h);
            }
            // ���ΰ� �� �� ���, ���θ� ���߰� ���θ� ���δ�(���͹ڽ� �¿�)
            else {
                float h = parentSize.y;
                float w = h * worldAspect;
                targetSize = new Vector2(w, h);
            }

            mapArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize.x);
            mapArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize.y);
            mapArea.anchoredPosition = Vector2.zero; // ��� ����
        }
        #endregion

        #region Mouse Interaction
        private void _OnPointerClick(PointerEventData eventData) {
            if (dragging) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(mapArea, eventData.position, eventData.pressEventCamera, out var local))
                return;
            var world = _ConvertMapToWorld(local);
            _MoveCameraToWorld(world);
        }

        private void _OnBeginDrag(PointerEventData eventData) {
            if (!allowDragNavigate) return;
            dragging = true;
            _OnDrag(eventData);
        }

        private void _OnDrag(PointerEventData eventData) {
            if (!allowDragNavigate) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(mapArea, eventData.position, eventData.pressEventCamera, out var local))
                return;
            var world = _ConvertMapToWorld(local);
            _MoveCameraToWorld(world);
        }

        private void _OnEndDrag(PointerEventData eventData) {
            dragging = false;
        }
        #endregion

#if UNITY_EDITOR
        [ContextMenu("Minimap/Snap Fit To World Aspect")]
        private void _Editor_SnapFit() {
            _RefreshWorldRect();
            _FitMinimapRectToWorldAspect();
            UnityEditor.EditorUtility.SetDirty(mapArea);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

        void OnDrawGizmosSelected() {
            var wr = _GetWorldRect();
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(wr.center, wr.size);
        }
#endif
    }
}
