using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gybe.Game
{
    public class ExperienceUI : MonoBehaviour
    {
        private IPlayerData _playerData;

        public TMP_Text currentLevelText;
        public TMP_Text nextLevelText;
        public Image loadingImage;
        
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        

        // Update is called once per frame
        void Update()
        {
            currentLevelText.text = _playerData.Level.ToString();
            nextLevelText.text = (_playerData.Level + 1).ToString();
            loadingImage.fillAmount = (float)_playerData.Experience / (float)_playerData.XPForNextLevel();
        }
    }
}