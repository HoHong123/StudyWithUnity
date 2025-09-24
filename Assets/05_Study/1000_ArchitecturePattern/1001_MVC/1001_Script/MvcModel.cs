#if UNITY_EDITOR
/* =========================================================
 * @Jason - PKH
 * HTitle �Ӽ� ��ũ��Ʈ�Դϴ�.
 * Header����� ��ü�ϱ� ���� �ۼ��Ǿ����ϴ�.
 * =========================================================
 */
#endif
using System;
using UnityEngine;

namespace Study.AS.Mvc {
    public class MvcModel : MonoBehaviour {
        public int Value { get; private set; }
        public event Action<int> Changed;

        public void Increment(int amount = 1) {
            if (amount == 0) return;
            Value += amount;
            Changed?.Invoke(Value);
        }

        public void ResetValue(int value = 0) {
            if (Value == value) return;
            Value = value;
            Changed?.Invoke(Value);
        }
    }
}
