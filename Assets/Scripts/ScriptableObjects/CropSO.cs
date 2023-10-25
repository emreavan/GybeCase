using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gybe.Game
{
    [CreateAssetMenu(menuName = "Gybe/Crop")]
    public class CropSO : ProductSO
    {
        public Sprite sprite;
        public PlantSO plant;
        public float spawnCoefficient;
        public int howManyProduct;
        public float readyTimeInMs;
        public int maximumProductCount;
        public int minimumLevel;
    }
}