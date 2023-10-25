using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gybe.Game
{
    public class Plant : Item
    {
        public List<Transform> spawnPositions;

        private List<Crop> _crops;
        
        [Range(0, 10)]
        public float scatterDistance = 3.0f;
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
                SpreadCrops();
                OnOnObjectCollected(gameObject);
            }
        }

        private void SpreadCrops()
        {
            foreach (var crop in _crops)
            {
                crop.gameObject.SetActive(true);
                Vector3? spawnPosition = NavMeshUtils.FindRandomPosition(transform.position, scatterDistance, -1);
                if (spawnPosition.HasValue)
                {
                    crop.Scatter(spawnPosition.Value);
                    //crop.transform.position = spawnPosition.Value;
                }
            }
        }
        public void SetCrops(List<Crop> cropList)
        {
            _crops = cropList;
            var count = Math.Min(_crops.Count, spawnPositions.Count);
            for (int i = 0; i < count; i++)
            {
                _crops[i].transform.position = spawnPositions[i].position;
            }

            for (int i = count; i < _crops.Count; i++)
            {   
                _crops[i].transform.position = transform.position;
                _crops[i].gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if(_crops == null)
                _crops = new List<Crop>();
        }

        private void OnDisable()
        {
            _crops.Clear();
            
        }
            
    }
}