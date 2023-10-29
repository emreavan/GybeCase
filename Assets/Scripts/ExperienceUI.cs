using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gybe.Game
{
    public class ExperienceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text nextLevelText;
        [SerializeField] private Image loadingImage;
        
        private IPlayerData _playerData;
        
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        
        void Update()
        {
            currentLevelText.text = _playerData.Level.ToString();
            nextLevelText.text = (_playerData.Level + 1).ToString();
            loadingImage.fillAmount = (float)_playerData.Experience / (float)_playerData.XPForNextLevel();
        }
    }
}