using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Order(List<Piece> list)
        {
            pieces = list;
        }
    }
}