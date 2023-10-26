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
        private Order _order;
        
        public event Action<Order> OnOrderCompleted;
        public DataListSO data;
        private IPlayerData _playerData;
        
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < _order.pieces.Count; i++)
            {
                textList[i].text = _playerData.CollectedCrops[_order.pieces[i].crop].ToString()
                + " / " + _order.pieces[i].quantity.ToString();
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
                imageList[i].sprite = data.FindCrop(_order.pieces[i].crop).sprite;
            }
        }

        public void ButtonClicked()
        {
            
        }

        private void OrderFinished()
        {
            OnOrderCompleted?.Invoke(_order);
        }
    }
}