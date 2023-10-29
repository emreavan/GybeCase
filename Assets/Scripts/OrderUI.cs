using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gybe.Game
{
    public class OrderUI : MonoBehaviour
    {
        public event Action<Order> OnOrderCompleted;
        
        [SerializeField] private List<TMP_Text> textList;
        [SerializeField] private List<Image> imageList;
        [SerializeField] private TMP_Text gold;
        [SerializeField] private CropsData cropsData;
        
        private Order _order;
        private Animator _animator;
        private bool? _previousIsReady;
        
        private IPlayerData _playerData;
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        
        void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _previousIsReady = false;
        }

        void Update()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            bool isReady = true;
            for (int i = 0; i < _order.pieces.Count; i++)
            {
                textList[i].text = _playerData.CollectedCrops[_order.pieces[i].crop].ToString()
                                   + " / " + _order.pieces[i].quantity.ToString();

                isReady = (isReady && (_playerData.CollectedCrops[_order.pieces[i].crop] - _order.pieces[i].quantity) >= 0)
                    ? true
                    : false;
            }
            
            if (isReady != _previousIsReady)
            {
                if (isReady)
                    _animator.SetTrigger("ready");
                else
                    _animator.SetTrigger("notReady");

                _previousIsReady = isReady;
            }
        }
        
        public void Initialize(Order newOrder)
        {
            _order = newOrder;

            for (int i = 0; i < _order.pieces.Count; i++)
            {
                textList[i].gameObject.SetActive(true);
                imageList[i].gameObject.SetActive(true);
                textList[i].text = _order.pieces[i].quantity.ToString();
                imageList[i].sprite = cropsData.dictionary[_order.pieces[i].crop].sprite;
            }

            gold.text = _order.gold.ToString();

            UpdateUI();
        }

        public void ButtonClicked()
        {
            foreach (var piece in _order.pieces)
            {
                if (piece.quantity > _playerData.CollectedCrops[piece.crop])
                {
                    return;
                }
            }

            OrderFinished();
            Destroy(gameObject);
        }

        private void OrderFinished()
        {
            OnOrderCompleted?.Invoke(_order);
        }
    }
}