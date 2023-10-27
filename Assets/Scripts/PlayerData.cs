using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public interface IPlayerData
    {
        void CollectCrop(CropSO crop, int amount);
        void RemoveCrop(CropSO crop, int amount);
        void GainExperience(int amount);
        
        int Gold { get; set;}
        int Experience { get; set;}
        int Level { get; set; }
        int Speed { get; set;}
        int CollectionRange { get; set;}
        
        int BaseExp { get; set;}
        Dictionary<ItemClassSO, int> CollectedCrops { get; }
        
        public event Action<int> OnLevelIncreased;
    }
    
    [System.Serializable]
    public class PlayerData : MonoBehaviour, IPlayerData
    {
        public int Gold { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int Speed { get; set; }
        public int CollectionRange { get; set; }
        public int BaseExp { get; set;}
        public DataListSO dataList;
        public event Action<int> OnLevelIncreased;
        public Dictionary<ItemClassSO, int> CollectedCrops { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            Level = 1;
            CollectedCrops = new Dictionary<ItemClassSO, int>();
            foreach (var var in dataList.cropList)
            {
                CollectedCrops.Add(var.itemClass, 0);
            }
        }

        public void CollectCrop(CropSO crop, int amount)
        {
            CollectedCrops[crop.itemClass] += amount;
        }

        public void RemoveCrop(CropSO crop, int amount)
        {
            CollectedCrops[crop.itemClass] -= amount;
        }
        
        public void GainExperience(int amount)
        {
            Experience += amount;

            while(Experience >= XPForNextLevel())
            {
                Experience -= XPForNextLevel();
                Level++;
                OnLevelIncreased?.Invoke(Level);
            }
        }
        
        int XPForNextLevel()
        {
            return BaseExp * Level * Level;
        }

    }
}