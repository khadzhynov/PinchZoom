using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    [SerializeField]
    private Transform _viewPoint;

    [SerializeField]
    private Transform _midPoint;

    [SerializeField]
    private Transform _controlledObject;

    [SerializeField]
    private float _offset;

    void LateUpdate()
    {
        _controlledObject.transform.position = _midPoint.position + (_midPoint.position - _viewPoint.position) * _offset;
    }
}
