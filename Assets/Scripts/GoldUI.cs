using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public class GoldUI : MonoBehaviour
    {
        public TMP_Text goldAmount;
        private IPlayerData _playerData;
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        
        void Update()
        {
            goldAmount.text = _playerData.Gold.ToString();
        }
    }
}