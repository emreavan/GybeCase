using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Gybe.Util;

namespace Gybe.Game
{
    public interface IProductManager
    {
        
    }
    public class ProductManager : MonoBehaviour, IProductManager
    {
        [SerializeField] private float spawnWaitTimeInSec = 0.5f;
        [SerializeField] private CropsData cropsData;
        [SerializeField] private PlantsData plantsData;
        
        private Dictionary<ItemClassSO, int> _productCounts;
        private List<ItemClassSO> _possibleCrops;
        private ObjectPool _pool;
        private float _currentWaitTime = 0.0f;
        
        private IPlayerData _playerData;
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
            _playerData.OnLevelIncreased += OnLevelIncreased;
        }
        
        private IGroundController _groundController;

        [Inject]
        public void Construct(IGroundController groundController)
        {
            _groundController = groundController;
        }
        
        
        void Start()
        {
            _productCounts = new Dictionary<ItemClassSO, int>();
            foreach (var val in cropsData.dictionary)
            { 
                _productCounts.Add(val.Value.itemClass, 0);
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
                if (cropsData.dictionary[itemClass].maximumProductCount > _productCounts[itemClass])
                {
                    SpawnProduct(itemClass);
                    return true;
                }
            }
            
            return false;
        } 
        
        private void SpawnProduct(ItemClassSO product)
        {
            var cropSO = cropsData.dictionary[product];
            if (cropSO != null)
            {
                var plantItemClass = cropSO.plantItemClass;
                
                var val = _pool.Get(plantItemClass);
                if (val != null)
                {
                    Vector3? spawnPosition = NavMeshUtils.FindRandomPosition(Random.Range(0.5f, _groundController.GroundRenderer.bounds.size.x / 2.0f), -1);
                    if (spawnPosition.HasValue)
                    {
                        val.transform.position = spawnPosition.Value;
                        
                        int cropCount =
                            Math.Min(cropSO.maximumProductCount - _productCounts[product],
                                cropSO.howManyProduct);

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
                _playerData.CollectCrop(cropVal.itemClass, 1);
            }
            
            item.OnObjectCollected -= DePool;
            _pool.Return(item.itemClass, obj);
        }
        
        private void CheckPossibleCrops()
        {
            foreach (var val in cropsData.dictionary)
            {
                if (val.Value.minimumLevel <= _playerData.Level && !_possibleCrops.Contains(val.Value.itemClass))
                {
                    _possibleCrops.Add(val.Value.itemClass);
                }
            }
        }
        
        private void OnLevelIncreased(int obj)
        {
            CheckPossibleCrops();
        }
    }
}

