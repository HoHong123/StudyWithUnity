using System;

namespace HUtil.Pooling {
    [Serializable]
    public class UnityPoolEntity {
        public int Key;
        public int Amount;
        public PoolableMono Prefab;
    }
}