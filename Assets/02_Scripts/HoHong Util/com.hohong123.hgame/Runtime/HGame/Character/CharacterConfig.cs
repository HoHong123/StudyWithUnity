#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * ���� ĳ���Ϳ� ���� �⺻ ĳ���� ������ ��� ��ũ��Ʈ�Դϴ�.
 * =========================================================
 */
#endif

using UnityEngine;

namespace HGame.Game.Character {
    public class CharacterConfig : BaseCharacterConfig {
        [SerializeField]
        protected Sprite charIcon;

        public Sprite Icon => charIcon;
    }
}