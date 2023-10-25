using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gybe.Game
{
    public class ProductSO : ScriptableObject
    {
        public ItemClassSO itemClass;
        public GameObject gameObject;
        public int pooledCount;
    }
}