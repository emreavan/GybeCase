using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gybe.Game
{
    public class OrderManager : MonoBehaviour
    {
        public CropsData cropsData;
        public int maxActiveOrders = 5;
        
        private List<Order> _activeOrders = new List<Order>();
        
        private IPlayerData _playerData;

        [Inject]
        private DiContainer Container;
        
        public OrderUI orderUIPrefab;
        
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            for(int i = 0; i < maxActiveOrders; i++)
                CreateRandomOrder();
        }

        public void CreateRandomOrder()
        {
            IEnumerable<KeyValuePair<ItemClassSO, CropSO>> results = cropsData.dictionary.Where(item => item.Value.minimumLevel <= _playerData.Level);

            var index = Random.Range(0, results.Count());
            var listResults = results.ToList();
            
            
            List<Order.Piece> list = new List<Order.Piece>();
            
            Order.Piece piece;
            piece.crop = listResults[index].Key;
            piece.quantity = Random.Range(0, 10 * _playerData.Level);
            
            list.Add(piece);
            
            Order newOrder = new Order(list, _playerData.BaseGoldForOrder * _playerData.Level, _playerData.BaseExpForOrder * _playerData.Level);
            
            var ui = Container.InstantiatePrefabForComponent<OrderUI>(orderUIPrefab, transform.GetChild(0));
            ui.Initialize(newOrder);
            ui.OnOrderCompleted += UiOnOnOrderCompleted;
        }

        private void UiOnOnOrderCompleted(Order order)
        {
            foreach (var piece in order.pieces)
            {
                _playerData.CollectedCrops[piece.crop] -= piece.quantity;
                _playerData.GainExperience(order.experience);
                _playerData.GainGold(order.gold);
                CreateRandomOrder();
            }
        }
    }
}