#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �̴ϸʿ� ǥ���Ǵ� ������Ʈ Ʈ��Ŀ ��ũ��Ʈ�Դϴ�.
 * 
 * ** ���� **
 * 1. �̴ϸʿ� ǥ���Ǿ���ϴ� ���� ĳ���Ϳ��� ������Ʈ�� �߰��մϴ�.
 * 2. ĳ���� �ݶ��̴��� �Է��մϴ�.
 * 3. 'useIcon'���� ĳ���� ������ Ȥ�� �⺻ ������ �̹����� ����մϴ�.
 * 4. 'scaleByCollider'�� ���� ĳ������ �ݶ��̴� ũ�⿡ ����Ͽ� �̴ϸʿ� ǥ���� �̹��� ũ�⸦ �����մϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using Sirenix.OdinInspector;
using HGame.Game.Character;

namespace HGame._2D.Map {
    [DisallowMultipleComponent]
    public class MinimapTrackable : MonoBehaviour {
        [Title("Config")]
        [SerializeField, ReadOnly]
        CharacterConfig config;

        [Title("References")]
        [SerializeField]
        Collider2D charCollider;

        [Title("Icon")]
        [SerializeField]
        bool useIcon = false;
        [SerializeField]
        bool scaleByCollider = false;
        [SerializeField]
        float iconSizeMin = 6f;
        [SerializeField]
        float iconSizeMax = 18f;

        [Title("Visibility")]
        [SerializeField]
        bool showWhenOutOfBounds = false;

        public bool UseIcon => useIcon;
        public bool ScaleByCollider => scaleByCollider;
        public bool ShowWhenOutOfBounds => showWhenOutOfBounds;
        public float IconSizeMin => iconSizeMin;
        public float IconSizeMax => iconSizeMax;
        public Sprite Icon => config.Icon;
        public Transform Target => transform;
        public Collider2D Collider => charCollider;

        public void Init(CharacterConfig config) {
            this.config = config;
        }
    }
}
