#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * �� ������Ʈ�� �˾��� ����� �Ŵ�����Ʈ ��ũ��Ʈ�Դϴ�.
 * 
 * ** ���� **
 * 1. �ش� ��ũ��Ʈ�� ��� �޴� �� �˾��Ŵ��� ��ũ��Ʈ�� �����մϴ�.
 * 2. �ڽ� �˾��Ŵ����� ������Ʈ�� ������ �� ������Ʈ�� �����մϴ�.
 * 3. �� �˾�â�� ��µ� �θ� �� ������Ʈ���� �����մϴ�.
 * + �� �������� �ϳ��� �����ϴ� ���� ��õ�մϴ�.
 * 4. �˾� �߻��� ����� ����� UI ������Ʈ�� 'background' ���� ����մϴ�.
 * 5. �⺻ '�ؽ�Ʈ, �̹���, ����' �˾��� ����մϴ�. (Demo���Ͽ� ����ֽ��ϴ�. �ʿ�� ���ϴ� �˾��� Ŀ�����Ͻø� �˴ϴ�.)
 * 
 * ** ���� **
 * 1. TextPopup�� ���, ��� �󵵰� ���� ������ ����Ͽ� �ϳ��� ������ �� �����մϴ�.
 * 2. �̹����� ���� �˾��� ���, ��� �󵵰� ���� ������ �����Ͽ� �ʿ��Ҷ� ��ü�� �����ϰ� ��� �� �ı��ϵ��� �����߽��ϴ�.
 * 3. 'logParent'�� �Ϲ� ���� �αװ� �ƴ� �����, �˶�, ��� �뵵�� �α׸� ������ �θ� ������Ʈ �Դϴ�.
 * 4. 'gameParent'�� ���ӿ� ���� �ǵ��� �˾�â�� �ö󰡴� �θ� ������Ʈ �Դϴ�.
 * 5. 'logHistory'�� �˾�â�� �ݺ� ����ɶ� ���� ����� �����͸� �����ϴ� �����̳��Դϴ�.
 * =========================================================
 */
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using HUtil.Core;
using HUtil.Logger;
using HUtil.Inspector;
using HUtil.UI.Spinner;

namespace HUtil.UI.Popup {
    public abstract class PopupManager<T> : SingletonBehaviour<T> where T : PopupManager<T> {
        #region Class
        [Serializable]
        public class LogQue {
            public int UID { get; private set; }
            public PopLevel Level { get; private set; }
            public string Title { get; private set; }
            public string Message { get; private set; }
            public Action OnClickAction { get; private set; }

            public LogQue(
                int uid, PopLevel level,
                string title, string message,
                Action onClickAction = null) {
                UID = uid;
                Level = level;
                Title = title;
                Message = message;
                OnClickAction = onClickAction;
            }

            public override string ToString() =>
                $"UID :: {UID}\n" +
                $"Level :: {Level}\n" +
                $"Title :: {Title}\n" +
                $"Message :: {Message}";
        }
        #endregion


        #region Member
        #region Static
        protected static SpinnerCtrl spinner;
        public static SpinnerCtrl Spinner => spinner;
        #endregion

        [HTitle("UI")]
        [SerializeField]
        protected GameObject background;

        [HTitle("Prefab")]
        [SerializeField]
        protected TextPopup textPrefab;
        [SerializeField]
        protected ImagePopup imagePrefab;
        [SerializeField]
        protected VideoPopup videoPrefab;

        [HTitle("Parents")]
        [SerializeField]
        protected Transform logParent;
        [SerializeField]
        protected Transform gameParent;
        [SerializeField]
        protected Transform primeParent;

        protected TextPopup textInstance = null;
        protected ImagePopup imgInstnace = null;
        protected VideoPopup vidInstnace = null;

        protected int creatStack = 0;
        protected Queue<LogQue> logHistory = new();

        public void DebugLogs() {
#if UNITY_EDITOR
            var builder = new System.Text.StringBuilder(512);
            builder.AppendLine($"[PopupManager Debug] ({GetType().Name}) {nameof(DebugLogs)}");
            builder.AppendLine($"- Background Active : {background != null && background.activeSelf}");
            builder.AppendLine($"- TextPopup Exists  : {textInstance != null} / Active: {(textInstance != null ? textInstance.IsActive : false)}");
            builder.AppendLine($"- ImagePopup Exists : {imgInstnace != null}");
            builder.AppendLine($"- VideoPopup Exists : {vidInstnace != null}");
            builder.AppendLine($"- Log Parent Childs : {(logParent != null ? logParent.childCount : -1)}");
            builder.AppendLine($"- Game Parent Childs: {(gameParent != null ? gameParent.childCount : -1)}");
            builder.AppendLine($"- Prime Parent Childs: {(primeParent != null ? primeParent.childCount : -1)}");
            builder.AppendLine($"- Created UID Stack : {creatStack}");
            builder.AppendLine($"- Log Count       : {logHistory.Count}");
            builder.AppendLine($"- Is All Closed?    : {_IsAllCose}");
            builder.AppendLine();

            if (logHistory.Count > 0) {
                int index = 0;
                foreach (var log in logHistory) {
                    builder.AppendLine($"[#{index}] UID:{log.UID} Level:{log.Level}");
                    builder.AppendLine($"    Title  : {log.Title}");
                    builder.AppendLine($"    Message: {log.Message}");
                    builder.AppendLine();
                    index++;
                }
            }
            else {
                builder.AppendLine("(Log history is empty)");
            }

            HLogger.Log(builder.ToString());
#endif
        }


        protected bool _IsAllCose => gameParent.childCount + logHistory.Count == 0;
#endregion


        protected override void Awake() {
            base.Awake();
            spinner = GetComponent<SpinnerCtrl>();
        }


        public void ShowLog(PopLevel level, string title, string message, Action onClickCancel = null) {
            int uid = ++creatStack;
            background.SetActive(true);
            logHistory.Enqueue(new(uid, level, title, message,onClickCancel));

            switch (level) {
            case PopLevel.Log: HLogger.Log($"[Log UID {uid}] {title} :: {message}"); break;
            case PopLevel.Warning: HLogger.Warning($"[Warning UID {uid}] {title} :: {message}"); break;
            case PopLevel.Alert: HLogger.Error($"[Alert UID {uid} ]  {title}  ::  {message}"); break;
            case PopLevel.Fatal: HLogger.Error($"[Fatal UID {uid} ]  {title}  ::  {message}"); break;
            default: HLogger.Error($"Log data invalid. Check log level({level.ToString()})"); break;
            }

            // Create one text popup
            if (textInstance == null) {
                textInstance = Instantiate(textPrefab, logParent);
                textInstance.OnClickCancel += _SetTextPopup;
                textInstance.Close();
            }

            if (!textInstance.IsActive) {
                _SetTextPopup();
            }
        }

        public void ShowImage(Sprite sprite, Action onClick = null) => ShowImage(sprite.texture, onClick);
        public void ShowImage(Texture texture, Action onClick = null) {
            imgInstnace = Instantiate(imagePrefab, gameParent);
            imgInstnace.SetUi(texture);
            imgInstnace.OnClickPanel += onClick;
        }

        public void ShowVideo(string address, Action onClick = null, int width = 0, int height = 0) {
            vidInstnace = Instantiate(videoPrefab, gameParent);
            vidInstnace.SetVideo(address, width, height);
            vidInstnace.OnClickPanel += onClick;
        }


        private void _SetTextPopup() {
            if (logHistory.Count == 0) {
                textInstance.Close();
                background.SetActive(false);
                return;
            }

            LogQue log = logHistory.Dequeue();
            textInstance.SetText(log.Title, log.Message, log.OnClickAction);
            textInstance.Open();
        }
    }
}