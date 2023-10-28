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
            goldAmount.text = FormatCoinAmount(_playerData.Gold);
        }
        
        string FormatCoinAmount(int amount)
        {
            if (amount < 1000)
                return amount.ToString();

            if (amount < 1000000) // Less than a million
                return (amount / 1000.0).ToString("0.#") + "K"; 

            if (amount < 1000000000) // Less than a billion
                return (amount / 1000000.0).ToString("0.#") + "M";

            // For amounts greater than or equal to a billion
            return (amount / 1000000000.0).ToString("0.#") + "B";
        }
    }
}