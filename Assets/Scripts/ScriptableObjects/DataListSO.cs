using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gybe.Game
{
    [CreateAssetMenu(menuName = "Gybe/Data List")]
    public class DataListSO : ScriptableObject
    {
        [SerializeField] public List<PlantSO> plantList = new List<PlantSO>();
        [SerializeField] public List<CropSO> cropList = new List<CropSO>();
        
        public PlantSO FindPlant(ItemClassSO itemClass)
        {
            return plantList.Find(item => item.itemClass == itemClass);
        }
         
        public CropSO FindCrop(ItemClassSO itemClass)
        {
            return cropList.Find(item => item.itemClass == itemClass);
        }

        public GameObject FindGameObject(ItemClassSO itemClass)
        {
            var val = cropList.Find(item => item.itemClass == itemClass);
            if (val != null)
                return val.gameObject;
            else
                return plantList.Find(item => item.itemClass == itemClass)?.gameObject;
        }

        public List<ProductSO> GetDataList()
        {
            List<ProductSO> dataList = new List<ProductSO>();
            
            for (int i = 0; i < plantList.Count; i++)
            {
                dataList.Add(plantList[i]);
            }
            
            for (int i = 0; i < cropList.Count; i++)
            {
                dataList.Add(cropList[i]);
            }

            return dataList;
        }
        
    }
}