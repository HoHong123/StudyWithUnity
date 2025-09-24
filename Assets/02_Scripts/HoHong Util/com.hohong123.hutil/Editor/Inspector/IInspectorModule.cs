
using UnityEditor;
using UnityEngine;

namespace HEditor.Inspector {
    /// <summary>모듈을 순서대로 호출해 필드를 소비/렌더하는 허브.</summary>
    public interface IInspectorModule {
        /// <summary>
        /// iterator가 가리키는 프로퍼티를 모듈이 처리하면 UI를 그리고,
        /// iterator를 "소비한 마지막 프로퍼티"로 이동시킨 뒤 true 반환.
        /// 처리 불가 시 false.
        /// </summary>
        public bool TryConsume(SerializedObject so, Object target, ref SerializedProperty iterator);
    }
}