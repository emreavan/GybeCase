using UnityEngine;
using UnityEngine.Serialization;

namespace Gybe.Game
{
    [CreateAssetMenu(menuName = "Gybe/Crop")]
    public class CropSO : ProductSO
    {
        public Sprite sprite;
        public ItemClassSO plantItemClass;
        public float spawnCoefficient;
        public int howManyProduct;
        [FormerlySerializedAs("readyTimeInMs")] public float readyTimeInSec;
        public int maximumProductCount;
        public int minimumLevel;
    }
}