#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * ���� ������Ʈ�� ���� ���� Ǯ���ý��� �Ŵ�����Ʈ �ý����� �����ϴ� ������Ʈ ��ũ��Ʈ�Դϴ�.
 * =========================================================
 */
#endif

using System.Collections.Generic;
using UnityEngine;
using Util.OdinCompat;

namespace Util.Pooling {
    public partial class UnityPoolConnector : MonoBehaviour {
        [HeaderOrTitle("Entities")]
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InfoBox("Must use 'IPoolable' prefab", Sirenix.OdinInspector.InfoMessageType.Warning)]
#else
        [Tooltip("Must use 'IPoolable' prefab")]
#endif
        List<UnityPoolEntity> poolEntity = new();

        private void Awake() {
            foreach (var entity in poolEntity) {
                UnityPoolMaster.Register(entity);
            }
        }

        private void OnDestroy() {
            foreach (var entity in poolEntity) {
                UnityPoolMaster.Remove(entity);
            }
        }
    }
}

#if UNITY_EDITOR
/* @Jason
 * Created at May. 29. 2025
 * 1. Create for example.
 */
#endif