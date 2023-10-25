using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;
    public float smoothSpeed = 0.125f;

    private Vector3 _offset;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if(player != null)
            _target = player.transform;
        else
        {
            Debug.LogError("Player is missing!");
            return;
        }
        _offset = transform.position - _target.position;
        
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = _target.position + _offset;
        desiredPosition.y = transform.position.y;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void UpdateOffset(Transform ground)
    {
        var pos = transform.position;
        
        // Get the size of the ground's renderer bounds
        Renderer groundRenderer = ground.GetComponent<Renderer>();
        if (groundRenderer)
        {
            float groundSize = Mathf.Max(groundRenderer.bounds.size.x, groundRenderer.bounds.size.z);
            float desiredOrthographicSize = (groundSize / 2) * 2.0f; // Divide by 2 because orthographicSize is half the height

            // Adjust the camera's orthographic size based on the ground size
            Camera.main.orthographicSize = desiredOrthographicSize;
        }
        
    }
}