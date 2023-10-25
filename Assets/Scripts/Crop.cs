using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public class Crop : Item
    {
        public CropSO cropSO;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnOnObjectCollected(gameObject);
            }
        }
    }
}