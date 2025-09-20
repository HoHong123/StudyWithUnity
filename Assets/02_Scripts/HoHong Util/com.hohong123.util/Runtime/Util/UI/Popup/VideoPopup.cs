using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Util.OdinCompat;

namespace Util.UI.Popup {
    public class VideoPopup : BasePopupUi {
        [HeaderOrTitle("Video")]
        [SerializeField]
        VideoPlayer video;
        [SerializeField]
        RenderTexture render;

        [HeaderOrTitle("Button")]
        [SerializeField]
        Button panelBtn;

        public event Action OnClickPanel;


        protected override void Start() {
            base.Start();
            closeBtn.onClick.AddListener(() => Destroy(panel));
            panelBtn.onClick.AddListener(() => OnClickPanel?.Invoke());
            video.Stop();
        }

        private void OnDisable() {
            video.Stop();
        }


        public void SetVideo(string url, int width = 0, int height = 0) {
            video.Stop();
            video.url = url;
            video.Play();

            if (width > 0) render.width = width;
            if (height > 0) render.height = height;
        }
    }
}