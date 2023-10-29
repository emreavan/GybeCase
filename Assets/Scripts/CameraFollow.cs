using System.Collections;
using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _target;
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private float scalingSpeed = 0.3f;

        private Vector3 _offset;
        private Camera _camera;
        private const float _referenceAspectRatio = 9f / 16f;

        private IGroundController _groundController;

        [Inject]
        public void Construct(IGroundController groundController)
        {
            _groundController = groundController;
            _groundController.OnGroundScaleIncreaseStart += OnGroundScaleIncreaseStart;
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            _camera = Camera.main;
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                _target = player.transform;
            else
            {
                Debug.LogError("Player is missing!");
                return;
            }

            _offset = transform.position - _target.position;

            StartCoroutine(ChangeOrtographicSize(new Vector3(1, 0, 1), _groundController.GroundRenderer.bounds.size));
        }

        private void LateUpdate()
        {
            Vector3 desiredPosition = _target.position + _offset;
            var position = transform.position;
            desiredPosition.y = position.y;

            Vector3 smoothedPosition = Vector3.Lerp(position, desiredPosition, smoothSpeed);
            position = smoothedPosition;
            transform.position = position;
        }

        private float CalculateOrthographicSize(Vector3 finalScale, Vector3 size)
        {
            float groundSize = Mathf.Max(size.x * finalScale.x, size.z * finalScale.z);

            float desiredOrthographicSizeForReference = (groundSize / 2f) * 1.8f;

            float currentAspectRatio = Screen.width / (float)Screen.height;

            float adjustedOrthographicSize =
                desiredOrthographicSizeForReference * (_referenceAspectRatio / currentAspectRatio);

            return adjustedOrthographicSize;
        }

        private IEnumerator ChangeOrtographicSize(Vector3 finalScale, Vector3 size)
        {
            float desiredOrthographicSize = CalculateOrthographicSize(finalScale, size);

            while (Mathf.Abs(desiredOrthographicSize - _camera.orthographicSize) > 0.1f)
            {
                _camera.orthographicSize =
                    Mathf.Lerp(_camera.orthographicSize, desiredOrthographicSize, scalingSpeed);
                yield return null;
            }

            _camera.orthographicSize = desiredOrthographicSize;
        }


        private void OnGroundScaleIncreaseStart(Vector3 finalScale, Vector3 size)
        {
            StartCoroutine(ChangeOrtographicSize(finalScale, size));
        }
    }
}