#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * ���� �÷ο� ���� ��� ��ũ��Ʈ�Դϴ�.
 * 
 * ** ���� **
 * 1. �ش� ��ũ��Ʈ�� ��� �޴� ��ũ��Ʈ�� �����մϴ�.
 * 2. �ڽ� ��ũ��Ʈ ������Ʈ�� �� ���� ������Ʈ�� �Է��մϴ�.
 * 3. �ڽ� ��ũ��Ʈ���� ���ϴ� ������ ������ �۾��� �����մϴ�.
 * 4. order�� ���� �� �۾��� �켱������ �����մϴ�.
 * 5. 'Util.Game.Flow.GameManager'�� ��ӹ޴� �� ������Ʈ�� ���ӸŴ��� ��ũ��Ʈ�� 'modules'�� �ڽ� ��ũ��Ʈ���� ����մϴ�.
 * Ps. ���� ��ũ��Ʈ�� Ȯ���� �� �ֽ��ϴ�.
 * =========================================================
 */
#endif

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HGame.Game.Flow {
    [Serializable]
    public abstract class BaseGameModule : MonoBehaviour, IGamePhaseModule {
        [SerializeField]
        int order = 0;

        public int Order => order;

        public virtual UniTask OnEnterPrepare(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
        public virtual UniTask OnEnterStart(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
        public virtual UniTask OnEnterRun(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
        public virtual UniTask OnEnterPause(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
        public virtual UniTask OnEnterOver(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
        public virtual UniTask OnEnterPost(GameContext context, CancellationToken ct) => UniTask.CompletedTask;
    }
}