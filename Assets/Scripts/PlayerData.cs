using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public interface IPlayerData
    {
        void CollectCrop(ItemClassSO itemClass, int amount);
        void DeductCrop(CropSO crop, int amount);
        void GainExperience(int amount);
        void GainGold(int amount);
        void SetSpeed(float speed);
        void SetCollectionRange(float range);

        int Gold { get; }
        int Experience { get; }
        int Level { get; }
        float Speed { get; }
        float CollectionRange { get; }
        int BaseExpForOrder { get; }
        int BaseGoldForOrder { get; }
        Dictionary<ItemClassSO, int> CollectedCrops { get; }
        
        event Action<int> OnLevelIncreased;
    }

    [System.Serializable]
    public class PlayerData : MonoBehaviour, IPlayerData
    {
        public int Gold { get; private set; }
        public int Experience { get; private set; }
        public int Level { get; private set; } = 1;
        public float Speed { get; private set; }
        public float CollectionRange { get; private set; }
        
        [SerializeField] private int baseExpForOrder;
        public int BaseExpForOrder => baseExpForOrder;
        
        [SerializeField] private int baseGoldForOrder;
        public int BaseGoldForOrder => baseGoldForOrder;
        
        [SerializeField] private CropsData cropsData;
        
        [SerializeField] private int baseExpNeededForLevel = 100;
        
        public event Action<int> OnLevelIncreased;
        
        public Dictionary<ItemClassSO, int> CollectedCrops { get; private set; }
        
        private void Awake()
        {
            CollectedCrops = new Dictionary<ItemClassSO, int>();
            foreach (var key in cropsData.dictionary.Keys)
            {
                CollectedCrops[key] = 0;
            }
        }
        
        public void CollectCrop(ItemClassSO itemClass, int amount)
        {
            if (CollectedCrops.ContainsKey(itemClass))
            {
                CollectedCrops[itemClass] += amount;
            }
        }

        public void DeductCrop(CropSO crop, int amount)
        {
            if (CollectedCrops.ContainsKey(crop.itemClass))
            {
                CollectedCrops[crop.itemClass] -= amount;
                if (CollectedCrops[crop.itemClass] < 0) CollectedCrops[crop.itemClass] = 0;
            }
        }
        
        public void GainExperience(int amount)
        {
            Experience += amount;

            while (Experience >= XPForNextLevel())
            {
                LevelUp();
            }
        }
    
        private void LevelUp()
        {
            Experience -= XPForNextLevel();
            Level++;
            OnLevelIncreased?.Invoke(Level);
        }

        private int XPForNextLevel()
        {
            return baseExpNeededForLevel * Level;
        }

        public void GainGold(int amount)
        {
            Gold += amount;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetCollectionRange(float range)
        {
            CollectionRange = range;
        }
    }
}