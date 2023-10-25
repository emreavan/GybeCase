using System;
using System.Collections;
using System.Collections.Generic;
using Gybe.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gybe.Game
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public GameObject prefab;
            public int size;

            public Pool(GameObject p, int s)
            {
                prefab = p;
                size = s;
            }
        }

        [FormerlySerializedAs("productList")] public DataListSO dataList;
        private List<Pool> _pools = new List<Pool>();
        private Dictionary<ScriptableObject, Stack<GameObject>> _poolDictionary;

        private void Awake()
        {
            var dataList = this.dataList.GetDataList();

            foreach (var data in dataList)
            {
                Pool pool = new Pool(data.gameObject, data.pooledCount);
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
                
                _poolDictionary.Add(pool.prefab.GetComponent<Item>().itemClass, objectPool);
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
                    if (pool.prefab.GetComponent<Item>().itemClass == type)
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
