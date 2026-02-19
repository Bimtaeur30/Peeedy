using UnityEngine;


    public interface IPoolable
    {
        public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject { get; }
        public void ResetItem();
    }
