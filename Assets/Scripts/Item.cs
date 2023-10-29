using UnityEngine;
using System;

namespace Gybe.Game
{
    public class Item : MonoBehaviour
    {
        public event Action<GameObject> OnObjectCollected;
        public ItemClassSO itemClass;

        protected virtual void OnOnObjectCollected(GameObject obj)
        {
            OnObjectCollected?.Invoke(obj);
        }
    }
}