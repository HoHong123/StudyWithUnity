#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�濡�� Ư�� ������Ʈ�� �浹 �̺�Ʈ�� ����ϴ� ��ũ��Ʈ�� �����߽��ϴ�.
 * 
 * ** ���� **
 * 1. ������� ���׸� Ÿ�Կ� �浹 �� ������ ������Ʈ�� �����մϴ�.
 * 2. ���� �浹�� �߻��ϸ� �浹 ������Ʈ�� �±׸� ���մϴ�.
 * + �±� ������� �������� ('useTag' ����)
 * 3. �浹 �� ������ ������Ʈ�� �������� �븮�ڸ� ���� ����� ��� ������Ʈ�� �����մϴ�.
 * 4. �����ͻ� �̺�Ʈ ����Ʈ�� ����� Ȱ��ȭ�� �� �ֽ��ϴ�.
 * =========================================================
 */
#endif

#if ODIN_INSPECTOR
using UnityEngine;
using Sirenix.OdinInspector;
using Util.OdinCompat;
#endif

namespace HGame._2D.Map {
    public abstract partial class BaseEventPoint<T> : MonoBehaviour where T : MonoBehaviour {
#if ODIN_INSPECTOR
        [HeaderOrTitle("Filter")]
        [SerializeField]
        protected bool useTag = true;
        [SerializeField, ShowIf("useTag")]
        protected string targetTag;

        [HeaderOrTitle("Collider")]
        [SerializeField, Required]
        protected Collider2D eventCollider;

#if UNITY_EDITOR
        [HeaderOrTitle("Debug")]
        [SerializeField]
        protected bool useDebug = true;
        [SerializeField, ShowIf("useDebug")]
        protected Color debugColor = Color.red;
#endif
#endif
    }
}
