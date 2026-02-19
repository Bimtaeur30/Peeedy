using UnityEngine;


    public abstract class PoolableMono : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => this != null ? gameObject : null; 

        public virtual void ResetItem() { }
    }
