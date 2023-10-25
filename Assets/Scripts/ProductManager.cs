using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


namespace Gybe.Game
{
    public class ProductManager : MonoBehaviour
    {
        public DataListSO dataList;
        private Dictionary<ItemClassSO, int> _productCounts;

        private ObjectPool _pool;
        
        void Start()
        {
            _productCounts = new Dictionary<ItemClassSO, int>();
            foreach (var val in dataList.cropList)
            { 
                _productCounts.Add(val.itemClass, 0);
            }

            _pool = GetComponent<ObjectPool>();
            if (_pool == null)
            {
                Debug.LogWarning("There is no pool!");
                return;
            }

        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.A)) // Example trigger
            {
                SpawnProductRandom(dataList.cropList[0].itemClass);
            }
        }
        
        
        private void SpawnProductRandom(ItemClassSO product)
        {
            var crop = dataList.FindCrop(product);
            if (crop != null)
            {
                var plantItemClass = crop.plant.itemClass;
                
                var val = _pool.Get(plantItemClass);
                if (val != null)
                {
                    Vector3? spawnPosition = NavMeshUtils.FindRandomPosition(Random.Range(0.5f, 10.0f), -1);
                    if (spawnPosition.HasValue)
                    {
                        val.transform.position = spawnPosition.Value;
                        

                        int cropCount =
                            Math.Min(dataList.FindCrop(product).maximumProductCount - _productCounts[product],
                                crop.howManyProduct);

                        List<GameObject> crops = new List<GameObject>();
                        for (int i = 0; i < cropCount; i++)
                        {
                            var valCrop = _pool.Get(product);
                            if (valCrop != null)
                            {
                                valCrop.GetComponent<Crop>().OnObjectCollected += Depool;
                                crops.Add(valCrop);
                            }
                        }
                        
                        var plant = val.GetComponent<Plant>();
                        plant.OnObjectCollected += Depool;
                        plant.SetCrops(crops);
                    }
                    else
                    {
                        _pool.Return(product, val);
                    }
                }
            }
        }

        private void Depool(GameObject obj)
        {
            var crop = obj.GetComponent<Item>();
            crop.OnObjectCollected -= Depool;
            _pool.Return(crop.itemClass, obj);
        }
    }
}

