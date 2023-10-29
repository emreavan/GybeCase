using TMPro;
using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text goldAmount;

        private IPlayerData _playerData;

        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        
        void Update()
        {
            UpdateGoldAmountDisplay();
        }
        
        private void UpdateGoldAmountDisplay()
        {
            goldAmount.text = FormatCoinAmount(_playerData.Gold);
        }
        
        private string FormatCoinAmount(int amount)
        {
            if (amount < 1_000) return amount.ToString();
            if (amount < 1_000_000) return $"{amount / 1_000.0:0.#}K";
            if (amount < 1_000_000_000) return $"{amount / 1_000_000.0:0.#}M";
            return $"{amount / 1_000_000_000.0:0.#}B";
        }
    }
}