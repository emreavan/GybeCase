using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public interface IPlayerData
    {
        void CollectCrop(CropSO crop);
        int Gold { get; set;}
        int Experience { get; set;}
        int Level { get; set; }
        int Speed { get; set;}
        int CollectionRange { get; set;}
        
        Dictionary<ItemClassSO, int> CollectedCrops { get; }
    }
    
    [System.Serializable]
    public class PlayerData : MonoBehaviour, IPlayerData
    {
        public int Gold { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int Speed { get; set; }
        public int CollectionRange { get; set; }
        public DataListSO dataList;
        
        public Dictionary<ItemClassSO, int> CollectedCrops { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            Experience = 1;
            CollectedCrops = new Dictionary<ItemClassSO, int>();
            foreach (var var in dataList.cropList)
            {
                CollectedCrops.Add(var.itemClass, 0);
            }
        }

        public void CollectCrop(CropSO crop)
        {
            CollectedCrops[crop.itemClass] += 1;
        }

    }
}