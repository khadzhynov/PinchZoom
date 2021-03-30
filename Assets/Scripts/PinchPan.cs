using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PinchPan
{
    [SerializeField]
    private float _dragOffset = 0.5f;

    [SerializeField]
    private float _elastity = 5f;

    private Camera _camera;
    private Bounds _bounds;
    private Dictionary<int, Vector3> _startOffsets = new Dictionary<int, Vector3>();
    private Vector3 _mouseStartOffset;

    public void Initialize(Bounds bounds, Camera camera)
    {
        _bounds = bounds;
        _camera = camera;
    }

    public void Apply()
    {
        if (Input.touchSupported)
        {
            TouchPan();
        }
        else
        {
            MousePan();
        }
    }

    private void MousePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartOffset = _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Pan(_mouseStartOffset, Input.mousePosition);
            return;
        }

        RelaxCamera(_camera.transform.position);
    }

    private void TouchPan()
    {
        if (Input.touchCount > 0)
        {
            ManageTouches();

            Touch panTouch = Input.GetTouch(0);
            if (panTouch.phase == TouchPhase.Moved)
            {
                Pan(_startOffsets[panTouch.fingerId], panTouch.position);
            }

            return;
        }

        RelaxCamera(_camera.transform.position);
    }

    private void ManageTouches()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _startOffsets.Add(touch.fingerId, _camera.ScreenToWorldPoint(touch.position));
            }
            else
            {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _startOffsets.Remove(touch.fingerId);
                }
            }
        }
    }

    private void Pan(Vector3 startOffset, Vector3 screenPosition)
    {
        Vector3 direction = startOffset - _camera.ScreenToWorldPoint(screenPosition);
        Vector3 panPosition = _camera.transform.position + direction;

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(panPosition.x, _bounds.min.x + _camera.orthographicSize * _camera.aspect, _bounds.max.x - _camera.orthographicSize * _camera.aspect),
            Mathf.Clamp(panPosition.y, _bounds.min.y + _camera.orthographicSize, _bounds.max.y - _camera.orthographicSize),
            _camera.transform.position.z);

        _camera.transform.position = clampedPosition;
    }

    private void RelaxCamera(Vector3 desiredPosition)
    {
        Vector2 maxDistanceToEdge = new Vector2(
                    _bounds.max.x - _camera.orthographicSize * _camera.aspect,
                    _bounds.max.y - _camera.orthographicSize);

        Vector2 minDistanceToEdge = new Vector2(
            _bounds.min.x + _camera.orthographicSize * _camera.aspect,
            _bounds.min.y + _camera.orthographicSize);

        desiredPosition = LimitPositionInsideEdges(
            desiredPosition, maxDistanceToEdge, minDistanceToEdge, _camera.transform.position.z, _dragOffset + 1);

        _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredPosition, Time.deltaTime * _elastity);
    }

    private Vector3 LimitPositionInsideEdges(Vector3 desiredPosition, Vector2 maxDistanceToEdge, Vector2 minDistanceToEdge, float zAxis, float offset)
    {
        desiredPosition = new Vector3(
                            Mathf.Clamp(desiredPosition.x, minDistanceToEdge.x / offset, maxDistanceToEdge.x / offset),
                            Mathf.Clamp(desiredPosition.y, minDistanceToEdge.y / offset, maxDistanceToEdge.y / offset),
                            zAxis);
        return desiredPosition;
    }

}
