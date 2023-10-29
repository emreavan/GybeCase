using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Gybe.Game
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private CropsData cropsData;
        [SerializeField] private int maxActiveOrders = 5;
        [SerializeField] private int minLevelForSecondPiece = 5;
        [SerializeField] private OrderUI orderUIPrefab;
        
        [Inject]
        private DiContainer Container;
        private List<Order> _activeOrders = new List<Order>();
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        
        private IPlayerData _playerData;
        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Start()
        {
            _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            Initialize();
        }

        public void Initialize()
        {
            for(int i = 0; i < maxActiveOrders; i++)
                CreateRandomOrder();
        }

        private void CreateRandomOrder()
        {
            IEnumerable<KeyValuePair<ItemClassSO, CropSO>> results = cropsData.dictionary.Where(item => item.Value.minimumLevel <= _playerData.Level);

            var index = Random.Range(0, results.Count());
            var listResults = results.ToList();
            
            List<Order.Piece> list = new List<Order.Piece>();
            
            Order.Piece piece;
            piece.crop = listResults[index].Key;
            piece.quantity = Random.Range(1, 10 * _playerData.Level);
            list.Add(piece);
            
            if (_playerData.Level >= minLevelForSecondPiece)
            {
                var indexSecond = Random.Range(0, results.Count());
                Order.Piece pieceSecond;
                pieceSecond.crop = listResults[indexSecond].Key;
                pieceSecond.quantity = Random.Range(1, 10 * _playerData.Level);
                list.Add(pieceSecond);
            }
            
            Order newOrder = new Order(list, _playerData.BaseGoldForOrder * _playerData.Level, _playerData.BaseExpForOrder * _playerData.Level);
            _activeOrders.Add(newOrder);
            
            var ui = Container.InstantiatePrefabForComponent<OrderUI>(orderUIPrefab, transform);
            ui.Initialize(newOrder);
            ui.OnOrderCompleted += UiOnOnOrderCompleted;
        }

        private void UiOnOnOrderCompleted(Order order)
        {
            foreach (var piece in order.pieces)
            {
                _activeOrders.Remove(order);
                _playerData.CollectedCrops[piece.crop] -= piece.quantity;
                _playerData.GainExperience(order.experience);
                _playerData.GainGold(order.gold);
                
                int count = _activeOrders.Count;
                for(int i = count; i < maxActiveOrders; i++)
                    CreateRandomOrder();
            }
        }
    }
}