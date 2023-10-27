using System;
using System.Collections.Generic;
using Gybe.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace Gybe.Game
{
    [CreateAssetMenu(menuName = "Gybe/Crops Data")]
    public class CropsData : SerializableDictionarySO<ItemClassSO, CropSO>{
    
    }
}