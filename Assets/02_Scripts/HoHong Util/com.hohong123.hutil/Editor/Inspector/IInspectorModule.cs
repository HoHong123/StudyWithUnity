
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector {
    /// <summary>����� ������� ȣ���� �ʵ带 �Һ�/�����ϴ� ���.</summary>
    public interface IInspectorModule {
        /// <summary>
        /// iterator�� ����Ű�� ������Ƽ�� ����� ó���ϸ� UI�� �׸���,
        /// iterator�� "�Һ��� ������ ������Ƽ"�� �̵���Ų �� true ��ȯ.
        /// ó�� �Ұ� �� false.
        /// </summary>
        public bool TryConsume(SerializedObject so, Object target, ref SerializedProperty iterator);
    }
}