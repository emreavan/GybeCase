using System;
using System.Collections.Generic;
using Gybe.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace Gybe.Game
{
    [CreateAssetMenu(menuName = "Gybe/Plants Data")]
    public class PlantsData : SerializableDictionarySO<ItemClassSO, PlantSO>{
    
    }
}