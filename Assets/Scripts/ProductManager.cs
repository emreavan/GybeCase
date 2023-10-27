using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Pool;
using Zenject;
using Random = UnityEngine.Random;


namespace Gybe.Game
{
    public class ProductManager : MonoBehaviour
    {
        
        private IPlayerData _playerData;

        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
            _playerData.OnLevelIncreased += OnLevelIncreased;
        }
        
        public DataListSO dataList;
        private Dictionary<ItemClassSO, int> _productCounts;
        private List<ItemClassSO> _possibleCrops;
        private ObjectPool _pool;
        public float spawnWaitTimeInSec = 0.5f;
        private float _currentWaitTime = 0.0f;
        void Start()
        {
            _productCounts = new Dictionary<ItemClassSO, int>();
            foreach (var val in dataList.cropList)
            { 
                _productCounts.Add(val.itemClass, 0);
            }

            _possibleCrops = new List<ItemClassSO>();

            CheckPossibleCrops();

            _pool = GetComponent<ObjectPool>();
            if (_pool == null)
            {
                Debug.LogWarning("There is no pool!");
                return;
            }

        }


        void Update()
        {
            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime > spawnWaitTimeInSec)
            {
                SpawnRandomProduct();
                _currentWaitTime -= spawnWaitTimeInSec;
            }
        }

        private bool SpawnRandomProduct()
        {
            for (int i = 0; i < 30; i++)
            {
                var index = Random.Range(0, _possibleCrops.Count);
                var itemClass = _possibleCrops[index];
                if (dataList.FindCrop(itemClass).maximumProductCount > _productCounts[itemClass])
                {
                    SpawnProduct(itemClass);
                    return true;
                }
            }

            return false;
        } 
        
        private void SpawnProduct(ItemClassSO product)
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

                        List<Crop> crops = new List<Crop>();
                        for (int i = 0; i < cropCount; i++)
                        {
                            var valCrop = _pool.Get(product);
                            if (valCrop != null)
                            {
                                var cropObj = valCrop.GetComponent<Crop>();
                                cropObj.OnObjectCollected += DePool;
                                crops.Add(cropObj);
                            }
                        }
                        
                        var plant = val.GetComponent<Plant>();
                        plant.OnObjectCollected += DePool;
                        plant.SetCrops(crops);
                        _productCounts[product] += cropCount;
                    }
                    else
                    {
                        _pool.Return(product, val);
                    }
                }
            }
        }

        private void DePool(GameObject obj)
        {
            var item = obj.GetComponent<Item>();

            if (obj.TryGetComponent<Crop>(out Crop cropVal))
            {
                _productCounts[cropVal.itemClass] -= 1;
                _playerData.CollectCrop(cropVal.cropSO, 1);
            }
            
            item.OnObjectCollected -= DePool;
            _pool.Return(item.itemClass, obj);
        }
        
        
        private void CheckPossibleCrops()
        {
            foreach (var val in dataList.cropList)
            {
                if (val.minimumLevel <= _playerData.Level && !_possibleCrops.Contains(val.itemClass))
                {
                    _possibleCrops.Add(val.itemClass);
                }
            }
        }
        
        private void OnLevelIncreased(int obj)
        {
            CheckPossibleCrops();
        }
    }
}

