using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Gybe.Game
{
    public class Crop : Item
    {
        public CropSO cropSO;
        
        [Range(1.0f, 10.0f)]
        [SerializeField] private float scatterAnimCoeff = 10f;

        private SphereCollider _collider;
        private Vector3 _localScale = new Vector3(-1,-1,-1);
        
        public void Scatter(Vector3 targetPosition)
        {
            StartCoroutine(MoveCrop(targetPosition));
        }
        
        public void StartScale()
        {
            transform.localScale = Vector3.zero;
            StartCoroutine(ScaleCrop());
        }
        
        private IEnumerator MoveCrop(Vector3 targetPosition)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, scatterAnimCoeff * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPosition;
            _collider.enabled = true;
        }

        private IEnumerator ScaleCrop()
        {
            while (Vector3.Distance(transform.localScale, _localScale) > 0.1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _localScale, 0.1f);
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = _localScale;
        }

        private void OnEnable()
        {
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();
            
            _collider.enabled = false;
            
            if(_localScale.x < 0)
                _localScale = transform.localScale;
            
            transform.localScale = _localScale;
                
            StartCoroutine(ScaleCrop());
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