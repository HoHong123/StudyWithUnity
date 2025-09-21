#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * ���� ĳ���Ϳ� ���� ���� �⺻�̵Ǵ� ĳ���� ������ ��� SO �����Դϴ�.
 * =========================================================
 */
#endif

using UnityEngine;
using HUtil.Inspector;

namespace HGame.Game.Character {
    public class BaseCharacterConfig : ScriptableObject {
        [HTitle("Meta")]
        [SerializeField]
        protected int uid;
        [SerializeField]
        protected string charName;

        public int UID => uid;
        public string Name => charName;
    }
}