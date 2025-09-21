#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
#endif
using UnityEngine;
using HUtil.Core;
using HUtil.Inspector;

namespace HUtil.Sound {
    public partial class SoundManager : SingletonBehaviour<SoundManager> {
#if ODIN_INSPECTOR
        [HTitle("Sound Data Allocation")]
        [DictionaryDrawerSettings(KeyLabel = "Audio Code", ValueLabel = "Audio Clip")]
        [SerializeField]
        Dictionary<int, SoundItem> soundDic = new();
#endif
    }
}
