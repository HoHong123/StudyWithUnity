#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �� ���� ��⿡ ���Ǵ� ����� Ÿ�̹� �Լ����� ���� �������̽��Դϴ�.
 * =========================================================
 */
#endif

using Cysharp.Threading.Tasks;
using System.Threading;
using HGame.Game.Flow;

namespace HGame.Game.Flow {
    public interface IGamePhaseModule {
        int Order { get; }
        UniTask OnEnterPrepare(GameContext ctx, CancellationToken ct);
        UniTask OnEnterStart(GameContext ctx, CancellationToken ct);
        UniTask OnEnterRun(GameContext ctx, CancellationToken ct);
        UniTask OnEnterPause(GameContext ctx, CancellationToken ct);
        UniTask OnEnterOver(GameContext ctx, CancellationToken ct);
        UniTask OnEnterPost(GameContext ctx, CancellationToken ct);
    }
}