using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public GameObject prefab;
            public int size;
            public ItemClassSO itemClass;
            public Pool(GameObject p, int s, ItemClassSO i)
            {
                prefab = p;
                size = s;
                itemClass = i;
            }
        }

        public CropsData cropsData;
        public PlantsData plantsData;
        private List<Pool> _pools = new List<Pool>();
        private Dictionary<ScriptableObject, Stack<GameObject>> _poolDictionary;

        private void Awake()
        {
            foreach (var val in cropsData.dictionary)
            { 
                Pool pool = new Pool(val.Value.gameObject, val.Value.pooledCount, val.Value.itemClass);
                _pools.Add(pool);
            }
            
            foreach (var val in plantsData.dictionary)
            { 
                Pool pool = new Pool(val.Value.gameObject, val.Value.pooledCount, val.Value.itemClass);
                _pools.Add(pool);
            }
            

            _poolDictionary = new Dictionary<ScriptableObject, Stack<GameObject>>();
            foreach (Pool pool in _pools)
            {
                Stack<GameObject> objectPool = new Stack<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject instance = Instantiate(pool.prefab);
                    instance.SetActive(false);
                    objectPool.Push(instance);
                }
                
                _poolDictionary.Add(pool.itemClass, objectPool);
            }
        }


        public GameObject Get(ScriptableObject type)
        {
            if (!_poolDictionary.ContainsKey(type))
            {
                Debug.LogWarning("Pool does not exist for this type: " + type);
                return null;
            }

            if (_poolDictionary[type].Count == 0)
            {
                // If pool is empty, create a new instance
                foreach (Pool pool in _pools)
                {
                    if (pool.itemClass == type)
                    {
                        GameObject instance = Instantiate(pool.prefab);
                        instance.SetActive(true);
                        return instance;
                    }
                }
            }

            GameObject existingInstance = _poolDictionary[type].Pop();
            existingInstance.SetActive(true);
            return existingInstance;
        }

        public void Return(ScriptableObject type, GameObject instance)
        {
            if (!_poolDictionary.ContainsKey(type))
            {
                Debug.LogWarning("Pool does not exist for this type: " + type);
                return;
            }

            instance.SetActive(false);
            _poolDictionary[type].Push(instance);
        }
    }
}
