using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gybe.Util;

namespace Gybe.Game
{
    public class Plant : Item
    {
        [SerializeField] private List<Transform> spawnPositions;
        [SerializeField] private  ParticleSystem readyParticle;
        
        [Range(0, 10)]
        public float scatterDistance = 3.0f;

        private List<Crop> _crops;
        private BoxCollider _collider;
        private Vector3 _localScale = new Vector3(-1,-1,-1);
        private const float ScaleCheck = 0.1f;
        private const float ScaleCoeff = 0.4f;
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
                }
            }
        }
        
        public void SetCrops(List<Crop> cropList)
        {
            float waitTime = 0.0f;
            _crops = cropList;
            var count = Math.Min(_crops.Count, spawnPositions.Count);
            for (int i = 0; i < count; i++)
            {
                waitTime = Mathf.Max(waitTime, _crops[i].cropSO.readyTimeInSec);
                _crops[i].transform.position = spawnPositions[i].position;
                _crops[i].StartScale();
            }

            for (int i = count; i < _crops.Count; i++)
            {   
                _crops[i].transform.position = transform.position;
                _crops[i].gameObject.SetActive(false);
            }

            StartAnimation(waitTime);
        }

        private void StartAnimation(float waitTime)
        {
            transform.localScale = Vector3.zero;
            StartCoroutine(ScalePlant(waitTime));
        }
        
        private void OnEnable()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider>();
            
            _collider.enabled = false;
            
            if(_localScale.x < 0)
                _localScale = transform.localScale;

            if(_crops == null)
                _crops = new List<Crop>();
        }

        private IEnumerator ScalePlant(float waitTime)
        {
            float startTime = Time.time;

            
            while (Vector3.Distance(transform.localScale, _localScale) > ScaleCheck)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _localScale, ScaleCoeff);
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = _localScale;
            
            while (startTime + waitTime > Time.time)
            {
                yield return new WaitForEndOfFrame();
            }
            
            _collider.enabled = true;
            readyParticle.Play();
        }
        
        private void OnDisable()
        {
            transform.localScale = _localScale;
            _crops.Clear();
        }
    }
}