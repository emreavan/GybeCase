using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gybe.Game
{
    public class OrderUI : MonoBehaviour
    {
        public List<TMP_Text> textList;
        public List<Image> imageList;
        public TMP_Text gold;
        private Order _order;
        private Animator _animator;
        public event Action<Order> OnOrderCompleted;
        public CropsData cropsData;
        private IPlayerData _playerData;
        
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            bool isReady = true;
            for (int i = 0; i < _order.pieces.Count; i++)
            {
                textList[i].text = _playerData.CollectedCrops[_order.pieces[i].crop].ToString()
                + " / " + _order.pieces[i].quantity.ToString();

                isReady = isReady && _playerData.CollectedCrops[_order.pieces[i].crop] - _order.pieces[i].quantity >= 0
                    ? true
                    : false;
            }
            
            if(isReady)
                _animator.SetTrigger("ready");
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