using System;
using UnityEngine.UI;

namespace HUtil.UI.Drop {
    public interface IDropUnit {
        public int UID { get; }
        public Toggle Toggle { get; }
        public Action<int> OnSelect { get; }
    }
}