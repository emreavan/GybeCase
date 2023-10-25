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

        private List<GameObject> _crops;
        // Start is called before the first frame update
        void Start()
        {
            _crops = new List<GameObject>();
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
                crop.GetComponent<SphereCollider>().isTrigger = false;
                Vector3? spawnPosition = NavMeshUtils.FindRandomPosition(transform.position, 5, -1);
                if (spawnPosition.HasValue)
                {
                    crop.transform.position = spawnPosition.Value;
                }
            }
        }
        public void SetCrops(List<GameObject> cropList)
        {
            _crops = cropList;
            var count = Math.Min(_crops.Count, spawnPositions.Count);
            for (int i = 0; i < count; i++)
            {
                _crops[i].transform.position = spawnPositions[i].position;
                _crops[i].GetComponent<SphereCollider>().isTrigger = true;
            }

            for (int i = count; i < _crops.Count; i++)
            {   
                _crops[i].gameObject.SetActive(false);
                _crops[i].GetComponent<SphereCollider>().isTrigger = true;
            }
        }

        void OnDisable()
        {
            _crops.Clear();
            
        }
            
    }
}