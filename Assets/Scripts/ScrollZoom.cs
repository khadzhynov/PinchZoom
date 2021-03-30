using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private RectTransform _container;

    [SerializeField]
    private PinchZoom _zoom;

    [SerializeField]
    private PinchPan _pan;

    private void Start()
    {
        var bounds = GetRectTransformBounds(_container);
        _zoom.Initialize(bounds, _camera);
        _pan.Initialize(bounds, _camera);
    }

    void LateUpdate()
    {
        _zoom.Apply();

        _pan.Apply();
    }

    private Bounds GetRectTransformBounds(RectTransform rectTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);
        Bounds bounds = new Bounds(rectTransform.position, Vector3.zero);

        foreach (var corner in worldCorners)
        {
            bounds.Encapsulate(corner);
        }

        return bounds;
    }
}
