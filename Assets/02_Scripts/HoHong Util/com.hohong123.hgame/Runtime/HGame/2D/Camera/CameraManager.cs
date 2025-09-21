#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * 2D ȯ�� ī�޶� �Ŵ�����Ʈ ��ũ��Ʈ.
 * =========================================================
 */
#endif

using UnityEngine;
using HUtil.Core;
using HUtil.Inspector;

namespace HGame._2D.Cam {
    public class CameraManager : SingletonBehaviour<CameraManager> {
        [HTitle("Camera Follow")]
        [SerializeField]
        CameraFollow follow;

        //[HTitle("Camera Effect")]
        // Add effect modules

        public void ResetFollow() => follow.ResetTarget();
        public void SetFollowTarget(Vector3 target) => follow.SetPosition(target);
        public void SetFollowTarget(Transform target) => follow.SetPosition(target);
    }
}


#if UNITY_EDITOR
/* Dev Log
 * @Jason - PKH
 * ī�޶� ���� �ý��� ���
 * TODO :: �߰� �ý��� ���� ���� (��鸲, ��ȯ, ����Ʈ ��)
 * =====================================
 */
#endif