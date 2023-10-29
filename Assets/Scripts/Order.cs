using System.Collections.Generic;

namespace Gybe.Game
{
    [System.Serializable]
    public class Order
    {
        [System.Serializable]
        public struct Piece
        {
            public ItemClassSO crop;
            public int quantity;
        }

        public List<Piece> pieces;
        public int gold;
        public int experience;
        public Order(List<Piece> list, int g, int exp)
        {
            pieces = list;
            gold = g;
            experience = exp;
        }
    }
}