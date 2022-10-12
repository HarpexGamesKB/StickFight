using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class DrawPath : MonoBehaviour
{
    [SerializeField] private LayerMask Layers;
    [SerializeField] private float MinDistance = 0.1f;

    private SplineComputer _spline;
    public SplinePoint[] points = new SplinePoint[0];
    private Vector3 _currentPosition;
    private Vector3 _lastPosition;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _spline = gameObject.AddComponent<SplineComputer>();

        //_spline.SetPoints(points);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (RayHit(out RaycastHit hit))
            {
                if (!hit.point.InDistance(_lastPosition, MinDistance))
                {
                    _lastPosition = _currentPosition;
                    _currentPosition = hit.point;
                    CreatePoint(_currentPosition);
                }
            }
        }
    }

    public void CreatePoint(Vector3 position)
    {
    }

    private bool RayHit(out RaycastHit hit)
    {
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layers))
        {
            return true;
        }
        return false;
    }
}