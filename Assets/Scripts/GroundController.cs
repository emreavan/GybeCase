using System;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public interface IGroundController
    {
        Vector3 LocalScale { get; }
        NavMeshSurface NavMeshSurface { get;  }
        Renderer GroundRenderer{ get; }
        
        event Action<Vector3, Vector3> OnGroundScaleIncreaseStart;
    }

    public class GroundController : MonoBehaviour, IGroundController
    {
        public Vector3 scaleCoefficient = new Vector3(0.1f, 0f, 0.1f);
        public float scalingSpeed = 0.1f;
        public NavMeshSurface NavMeshSurface { get; private set; }
        public Vector3 LocalScale { get; private set; }
        public  Renderer GroundRenderer{ get; private set; }
        public event Action<Vector3, Vector3> OnGroundScaleIncreaseStart;
        
        private IPlayerData _playerData;

        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
            _playerData.OnLevelIncreased += OnLevelIncreased;
        }

        private void Start()
        {
            NavMeshSurface = GetComponentInChildren<NavMeshSurface>();
            if (!NavMeshSurface)
            {
                Debug.LogError("NavMeshSurface component is missing!");
                enabled = false;
                return;
            }

            LocalScale = transform.localScale;
            GroundRenderer = NavMeshSurface.GetComponent<Renderer>();
        }

        private System.Collections.IEnumerator ScaleGround()
        {
            var originalScale = transform.localScale;
            var finalScale = originalScale + scaleCoefficient;
            OnGroundScaleIncreaseStart?.Invoke(new Vector3(1,0,1) + scaleCoefficient, GroundRenderer.bounds.size);
            float progress = 0;

            while (progress < 1)
            {
                transform.localScale = Vector3.Lerp(originalScale, finalScale, progress);
                LocalScale = transform.localScale;
                progress += scalingSpeed * Time.deltaTime;
                yield return null;
            }

            transform.localScale = finalScale; // Ensure target scale is reached
            LocalScale = transform.localScale;
            RebakeNavMesh();
        }

        private void RebakeNavMesh()
        {
            NavMeshSurface.BuildNavMesh();
        }

        private void OnLevelIncreased(int obj)
        {
            StartCoroutine(ScaleGround());
        }

    }
}
