#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 싱글톤 패턴 지원 스크립트
 * =========================================================
 */
#endif

#if ODIN_INSPECTOR
using OdinBehaviourBase = Sirenix.OdinInspector.SerializedMonoBehaviour;
#else
using OdinBehaviourBase = UnityEngine.MonoBehaviour;
#endif
using UnityEngine;
using HUtil.Logger;
using HUtil.Inspector;

namespace HUtil.Core {
    public class SingletonBehaviour<T> : OdinBehaviourBase where T : SingletonBehaviour<T> {
        [HTitle("Singleton")]
        [SerializeField]
        bool dontDestroyOnLoad;

        protected static T instance = null;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindFirstObjectByType(typeof(T)) as T;
                    if (instance == null) {
                        HLogger.Log("Nothing " + instance.ToString());
                        return null;
                    }
                }
                return instance;
            }
        }

        public static bool HasInstance => instance != null;


        // Use this for initialization
        protected virtual void Awake() {
            if (dontDestroyOnLoad) {
                DontDestroyOnLoad(gameObject);
            }

            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }

            instance = (T)this;
        }
    }
}