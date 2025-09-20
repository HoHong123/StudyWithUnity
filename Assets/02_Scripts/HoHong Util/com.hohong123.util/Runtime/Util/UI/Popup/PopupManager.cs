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
using Util.Logger;
using Util.Core;
using Util.OdinCompat;

namespace Util.UI.Popup {
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
        [HeaderOrTitle("UI")]
        [SerializeField]
        protected GameObject background;

        [HeaderOrTitle("Prefab")]
        [SerializeField]
        protected TextPopup textPrefab;
        [SerializeField]
        protected ImagePopup imagePrefab;
        [SerializeField]
        protected VideoPopup videoPrefab;

        [HeaderOrTitle("Parents")]
        [SerializeField]
        protected Transform logParent;
        [SerializeField]
        protected Transform gameParent;

        [HeaderOrTitle("Logs")]
        [SerializeField]
        protected Queue<LogQue> logHistory = new();

        protected TextPopup textInstance = null;
        protected ImagePopup imgInstnace = null;
        protected VideoPopup vidInstnace = null;

        protected int creatStack = 0;


        protected bool _IsAllCose => gameParent.childCount + logHistory.Count == 0;
        #endregion


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