using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Vector3 scaleCoefficient = new Vector3(0.1f, 0f, 0.1f);
    public float scalingSpeed = 0.1f;

    private NavMeshSurface _navMeshSurface;

    private void Start()
    {
        _navMeshSurface = GetComponentInChildren<NavMeshSurface>();
        if (!_navMeshSurface)
        {
            Debug.LogError("NavMeshSurface component is missing!");
            enabled = false;
            return;
        }
        
    }
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) // Example trigger
        {
            ScaleAndRebake();
        }
    }

    public void ScaleAndRebake()
    {
        StartCoroutine(ScaleGround());
    }

    private System.Collections.IEnumerator ScaleGround()
    {
        var originalScale = transform.localScale;
        var finalScale = originalScale + scaleCoefficient;
        float progress = 0;

        while (progress < 1)
        {
            transform.localScale = Vector3.Lerp(originalScale, finalScale, progress);
            progress += scalingSpeed * Time.deltaTime;
            Camera.main.GetComponent<CameraFollow>().UpdateOffset(_navMeshSurface.transform);
            yield return null;
        }

        transform.localScale = finalScale; // Ensure target scale is reached
        RebakeNavMesh();
    }

    private void RebakeNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }
}
