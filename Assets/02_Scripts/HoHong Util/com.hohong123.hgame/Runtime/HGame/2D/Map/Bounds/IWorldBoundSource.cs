#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2Dȯ�� 'MapManager' �̴ϸ� ��ɿ� �̴ϸ� ������ �����ϱ� ���� �������̽��Դϴ�.
 * �ٿ���� ������ ������ ��� ������Ʈ�� �����ϱ� ���� ���˴ϴ�.
 * =========================================================
 */
#endif

using UnityEngine;

namespace HGame._2D.Map {
    public interface IWorldBoundSource {
        bool TryGetWorldRect(out Rect rect);
    }
}