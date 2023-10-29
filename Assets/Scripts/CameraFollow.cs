using System;
using System.Collections;
using System.Collections.Generic;
using Gybe.Game;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;
    public float smoothSpeed = 0.125f;
    public float scalingSpeed = 0.3f;
    
    private Vector3 _offset;
    
    private IGroundController _groundController;
    private Camera _camera;

    [Inject]
    public void Construct(IGroundController groundController)
    {
        _groundController = groundController;
        _groundController.OnGroundScaleIncreaseStart += OnGroundScaleIncreaseStart;
    }
    
    private void Start()
    {
        _camera = Camera.main;
        var player = GameObject.FindWithTag("Player");
        if(player != null)
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
    
    private System.Collections.IEnumerator ChangeOrtographicSize(Vector3 finalScale, Vector3 size)
    {
        float groundSize = Mathf.Max(size.x * finalScale.x, size.z * finalScale.z);
        float desiredOrthographicSize = (groundSize / 2) * 1.8f;
        

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