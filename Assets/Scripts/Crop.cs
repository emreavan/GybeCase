using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gybe.Game
{
    public class Crop : Item
    {
        private SphereCollider _collider;
        public CropSO cropSO;

        [Range(0, 1.0f)]
        public float scatterAnimCoeff = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Scatter(Vector3 targetPosition)
        {
            StartCoroutine(MoveCrop(targetPosition));
        }
        
        private IEnumerator MoveCrop(Vector3 targetPosition)
        {
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, scatterAnimCoeff);
                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPosition;
            _collider.enabled = true;
        }
    
        private void OnDisable()
        {
            
        }

        private void OnEnable()
        {
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();
            
            _collider.enabled = false;
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