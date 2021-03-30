using System;
using UnityEngine;

[Serializable]
public class PinchZoom
{
    [SerializeField]
    private float _maxZoom = 6f;

    [SerializeField]
    private float _minZoom = 2f;

    [SerializeField]
    private float _zoomOffset = 0.1f;

    [SerializeField]
    private float _elastity = 5f;

    [SerializeField]
    private float _sensetivity = 0.01f;

    private Bounds _bounds;
    private Camera _camera;
    private float _minScale;

    public void Initialize(Bounds bounds, Camera camera)
    {
        Input.multiTouchEnabled = true;
        _bounds = bounds;
        _camera = camera;
        _minScale = GetMinCameraOrthoSizeSize();
    }

    public float ZoomFactor => _camera.orthographicSize / (_minScale / _minZoom);

    public void Apply()
    {
        float minZoom = _minScale / _minZoom;
        float maxZoom = _minScale / _maxZoom;

        if (Input.mouseScrollDelta != Vector2.zero || (Input.touchCount >= 2))
        {
            Zoom(minZoom, maxZoom);
        }
        else
        {
            Relax(minZoom, maxZoom);
        }
    }

    private void Relax(float minZoom, float maxZoom)
    {
        float targetSize = Mathf.Clamp(_camera.orthographicSize, maxZoom * (1 + _zoomOffset), minZoom / (1 + _zoomOffset));
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetSize, Time.deltaTime * _elastity);
    }

    private void Zoom(float minZoom, float maxZoom)
    {
        float zoomDelta = GetZoomDelta() * _sensetivity;

        if (zoomDelta < 0)
        {
            _camera.orthographicSize = Mathf.Max(maxZoom, _camera.orthographicSize + zoomDelta);
        }
        else
        {
            _camera.orthographicSize = Mathf.Min(minZoom, _camera.orthographicSize + zoomDelta);
        }
    }

    private float GetZoomDelta()
    {
        if (Input.touchSupported)
        {
            return GetPinchDelta();
        }
        return Input.mouseScrollDelta.y * 10;
    }

    private float GetPinchDelta()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        Vector2 touch1Previous = touch1.position - touch1.deltaPosition;
        Vector2 touch2Previous = touch2.position - touch2.deltaPosition;

        float oldDistance = Vector2.Distance(touch1Previous, touch2Previous);
        float newDistance = Vector2.Distance(touch1.position, touch2.position);


        return oldDistance - newDistance;
    }

    private float GetMinCameraOrthoSizeSize()
    {
        float contentAspect = _bounds.size.x / _bounds.size.y;

        if (contentAspect > _camera.aspect)
        {
            return _bounds.extents.y;
        }
        else
        {
            return _bounds.size.x * Screen.height / Screen.width * 0.5f;
        }
    }
}
