#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �� ������Ʈ�� ���Ǵ� ���̽� ���ӸŴ��� ��ũ��Ʈ �Դϴ�.
 * 
 * ** ���� **
 * 1. 'GameManager'�� ��� �޴� �ڽ� ���ӸŴ��� ��ũ��Ʈ�� �����մϴ�.
 * 2. �� ������Ʈ�� �ڽ� ���ӸŴ����� ����մϴ�.
 * 3. �ʿ信 ���� 'BaseGameModule'�� ��� ���� �ڽ� ��ũ��Ʈ���� 'modules'����Ʈ�� ����մϴ�.
 * =========================================================
 */
# endif

using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util.Core;
using Util.OdinCompat;

namespace HGame.Game.Flow {
    public class GameManager<TSelf> : SingletonBehaviour<TSelf>
    where TSelf : GameManager<TSelf> {
        [HeaderOrTitle("Flow")]
        [SerializeField]
        protected bool autoPrepareOnStart = true;
        [SerializeField]
        protected GamePhaseType phase = GamePhaseType.None;
        [SerializeField]
        [Tooltip("Modules (children or same GameObject)")]
        List<BaseGameModule> modules = new();

        GameContext context = new GameContext();
        CancellationTokenSource phaseCts;

        public GamePhaseType Phase => phase;


        protected override void Awake() {
            base.Awake();
            modules.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        protected virtual void OnEnable() { }

        protected virtual void Start() {
            if (autoPrepareOnStart) GamePrepareAsync();
        }

        protected virtual void OnDisable() {
            phaseCts?.Cancel();
            phaseCts?.Dispose();
            phaseCts = null;
            phase = GamePhaseType.None;
        }


        public UniTask GamePrepareAsync() => SwitchAsync(GamePhaseType.Prepare);
        public UniTask GameStartAsync() => SwitchAsync(GamePhaseType.Start);
        public UniTask GameRunAsync() => SwitchAsync(GamePhaseType.Running);
        public UniTask GameOverAsync() => SwitchAsync(GamePhaseType.Over);
        public UniTask GamePauseAsync() => SwitchAsync(GamePhaseType.Pause);


        protected async UniTask SwitchAsync(GamePhaseType phase) {
            if (Phase == phase) return;
            this.phase = phase;

            phaseCts?.Cancel();
            phaseCts?.Dispose();
            phaseCts = new CancellationTokenSource();
            var ct = phaseCts.Token;

            switch (phase) {
            case GamePhaseType.Prepare:
                foreach (var m in modules)
                    await m.OnEnterPrepare(context, ct);
                break;
            case GamePhaseType.Start:
                foreach (var m in modules)
                    await m.OnEnterStart(context, ct);
                break;
            case GamePhaseType.Running:
                foreach (var m in modules)
                    await m.OnEnterRun(context, ct);
                break;
            case GamePhaseType.Pause:
                foreach (var m in modules)
                    await m.OnEnterPause(context, ct);
                break;
            case GamePhaseType.Over:
                foreach (var m in modules)
                    await m.OnEnterOver(context, ct);
                break;
            case GamePhaseType.Post:
                foreach (var m in modules)
                    await m.OnEnterPost(context, ct);
                break;
            }
        }
    }
}