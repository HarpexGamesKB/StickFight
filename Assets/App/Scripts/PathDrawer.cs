using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Line
{
    public LineRenderer lineRenderer;
    public float YOffset;
    public List<Vector3> points;

    private Material _material;
    private float _offsetSpeed;

    
    public int LinePointsCount
    {
        get
        {
            if (lineRenderer == null)
            {
                return 0;
            }
            return lineRenderer.positionCount;
        }
        set
        {
            if (lineRenderer == null)
            {
                return;
            }
            lineRenderer.positionCount = value;
        }
    }
    
    public void CutFromStart()
    {
        if (LinePointsCount < 2) return;
        Vector3[] positions = new Vector3[LinePointsCount];
        lineRenderer.GetPositions(positions);

        float Dist = Vector3.Distance(positions[0], positions[1]);
        points.Clear();
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 pos = positions[i + 1];
            lineRenderer.SetPosition(i, positions[i + 1]);
            points.Add(pos);
        }

        LinePointsCount--;
        _material.mainTextureOffset += new Vector2(Dist * _offsetSpeed, 0);
    }

    public void CutFromSecond()
    {
        if (LinePointsCount < 2) return;
        Vector3[] positions = new Vector3[LinePointsCount];
        lineRenderer.GetPositions(positions);

        points.Clear();
        for (int i = 1; i < positions.Length - 1; i++)
        {
            Vector3 pos = positions[i + 1];
            lineRenderer.SetPosition(i, positions[i + 1]);
            points.Add(pos);
        }

        LinePointsCount--;
    }

    public void ChangeMaterialOffset(float value)
    {
        _material.mainTextureOffset += new Vector2(value, 0);
    }

    public Line(Material material, float width, float offsetSpeed)
    {
        Init(material, width, offsetSpeed, Color.white);
    }

    public Line(Material material, float width, float offsetSpeed, Color color)
    {
        Init(material, width, offsetSpeed, color);
    }

    private void Init(Material material, float width, float offsetSpeed, Color color)
    {
        _offsetSpeed = offsetSpeed;

        points = new List<Vector3>();

        GameObject go = new GameObject();

        go.transform.eulerAngles = new Vector3(90, 0, 0);

        lineRenderer = go.AddComponent<LineRenderer>();

        _material = new Material(material);
        _material.CopyPropertiesFromMaterial(material);
        if (color != Color.white)
        {
            _material.color = color;
        }
        lineRenderer.material = _material;
        lineRenderer.numCapVertices = 5;

        lineRenderer.textureMode = LineTextureMode.Tile;

        lineRenderer.positionCount = 0;

        lineRenderer.startWidth = width;

        lineRenderer.numCornerVertices = 6;

        lineRenderer.alignment = LineAlignment.TransformZ;
    }
}

public static class Curver
{
    //arrayToCurve is original Vector3 array, smoothness is the number of interpolations.
    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }
}

public class PathDrawer : Singleton<PathDrawer>
{
    [SerializeField] private LayerMask Layers;
    [SerializeField] private float MinDistance = 0.1f;
    [SerializeField] private float LineOffsetY = .05f;
    [SerializeField] private float LineWidth = .05f;
    [SerializeField] private Material LineMaterial;
    [SerializeField] private float OffsetSpeed;
    [SerializeField] private bool Debug;
    public bool allowedDraw = true;

    private Camera _camera;

    private Vector3 _currentPosition;
    private Vector3 _lastPosition;

    //private List<Line> _lines;
    private Line _currentLine;

    private Transform _reference;

    public event UnityAction<Vector3[]> OnPointAdded;
    public Enemy Enemy;
    private new void Awake()
    {
        _camera = Camera.main;
        _lastPosition = Vector3.one * -100;
        //_lines = new List<Line>();
    }

    private const int _countOfSmoothPoints = 3;
    private int _countOfPlacedPoints = 0;

    private void Update()
    {
        if (Debug && Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (_countOfPlacedPoints < _countOfSmoothPoints && _currentLine != null && _currentLine.lineRenderer != null && Input.GetMouseButton(0))
        {
            if (RayHit(out RaycastHit hit))
            {
                if (!hit.point.InDistance(_lastPosition, MinDistance))
                {
                    _currentPosition = hit.point + hit.normal * _currentLine.YOffset;
                    _lastPosition = _currentPosition;
                    CreatePoint(_currentPosition);

                    Enemy.AddOneStep();
                    Vibrator.Vibrate(25);
                    _countOfPlacedPoints++;
                    SendEvent();
                }
            }
            if (_countOfPlacedPoints == _countOfSmoothPoints)
            {
                if (_currentLine.LinePointsCount > 2)
                {
                    int oldPositionsCount = _currentLine.LinePointsCount; // 3  ||  7

                    Vector3[] positions = new Vector3[_countOfSmoothPoints]; // len = 3  ||  len = 3

                    for (int i = 0; i < _countOfSmoothPoints; i++)
                    {
                        positions[i] = _currentLine.lineRenderer.GetPosition(oldPositionsCount - _countOfSmoothPoints + i);
                    }

                    Vector3[] newPositions = Curver.MakeSmoothCurve(positions, 2.0f); // len = 6  ||  len = 6

                    _currentLine.LinePointsCount = oldPositionsCount + newPositions.Length - positions.Length;// 3 + 6 - 3 = 6  ||  7 + 6 - 3 = 10

                    for (int i = 0; i < newPositions.Length; i++) // i = 3 - 3 / i < 6  ||  7 - 3 = 4 / i < 10
                    {
                        if (oldPositionsCount - _countOfSmoothPoints + i > _currentLine.points.Count - 1)
                        {
                            _currentLine.points.Add(newPositions[i]);
                        }
                        else
                        {
                            _currentLine.points[i] = newPositions[i];
                        }
                        _currentLine.lineRenderer.SetPosition(oldPositionsCount - _countOfSmoothPoints + i, newPositions[i]);
                    }
                    _countOfPlacedPoints = 2;
                    SendEvent();
                }
                else
                {
                    _countOfPlacedPoints = _currentLine.LinePointsCount;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _countOfPlacedPoints = 0;
            _currentLine = null;
        }
    }

    private void SendEvent()
    {
        Vector3[] positions = new Vector3[_currentLine.LinePointsCount];
        _currentLine.lineRenderer.GetPositions(positions);
        OnPointAdded?.Invoke(positions);
    }

    public void CreatePoint(Vector3 position)
    {
        if (!allowedDraw) return;
        CreatePoint(position, _currentLine);
    }

    public void CreatePoint(Vector3 position, Line line)
    {
        if (!allowedDraw) return;
        if (line.LinePointsCount == 1 && _reference != null)
        {
            line.lineRenderer.SetPosition(0, _reference.position);
        }
        line.points.Add(position);
        line.LinePointsCount = line.points.Count;
        line.lineRenderer.SetPosition(line.LinePointsCount - 1, position + Vector3.up * LineOffsetY);
    }

    public Line DrawLine(LineRenderer lineRenderer, Material material, float lineWidth, UnityAction callback = null)
    {
        if (!allowedDraw) return null;
        Line line = CreateLine(material, lineWidth, false);
        StartCoroutine(AnimatesLineCreation(lineRenderer, line, callback));
        return line;
    }

    public void ClearLine()
    {
        _currentLine = null;
    }

    public Line CopyLine(LineRenderer lineRenderer, Material material, float lineWidth)
    {
        if (!allowedDraw) return null;
        Line line = CreateLine(material, lineWidth, false);
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            CreatePoint(positions[i], line);
        }
        return line;
    }

    private IEnumerator AnimatesLineCreation(LineRenderer lineRenderer, Line line, UnityAction callback = null)
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            yield return null;
            CreatePoint(positions[i], line);
        }
        callback?.Invoke();
    }

    public Line CreateLine()
    {
        if (!allowedDraw) return null;
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed);
        _currentLine = line;
        return line;
    }

    public Line CreateLine(Material material, float lineWidth, bool doCurrent = true)
    {
        if (!allowedDraw) return null;
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(material, lineWidth, OffsetSpeed);
        if (doCurrent)
        {
            _currentLine = line;
        }
        return line;
    }

    public Line CreateLine(Transform reference, Color color, float yOffset = 0)
    {
        if (!allowedDraw) return null;
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //CreatePoint(firstPoint);
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed, color);
        _currentLine = line;
        _currentLine.YOffset = yOffset;
        _reference = reference;
        return line;
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

    /*[SerializeField] private LayerMask Layers;
    [SerializeField] private float MinDistance = 0.1f;
    [SerializeField] private float LineOffsetY = .05f;
    [SerializeField] private float LineWidth = .05f;
    [SerializeField] private Material LineMaterial;
    [SerializeField] private float OffsetSpeed;
    [SerializeField] private bool Debug;

    private Camera _camera;

    private Vector3 _currentPosition;
    private Vector3 _lastPosition;

    //private List<Line> _lines;
    private Line _currentLine;

    private Transform _reference;

    public event UnityAction<Vector3[]> OnPointAdded;

    private void Awake()
    {
        _camera = Camera.main;
        _lastPosition = Vector3.one * -100;
        //_lines = new List<Line>();
    }

    private const int _countOfSmoothPoints = 3;
    private int _countOfPlacedPoints = 0;

    private void Update()
    {
        if (Debug && Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (_countOfPlacedPoints < _countOfSmoothPoints && _currentLine != null && _currentLine.lineRenderer != null && Input.GetMouseButton(0))
        {
            if (RayHit(out RaycastHit hit))
            {
                if (!hit.point.InDistance(_lastPosition, MinDistance))
                {
                    _currentPosition = hit.point + hit.normal * _currentLine.YOffset;
                    _lastPosition = _currentPosition;
                    CreatePoint(_currentPosition);
                    _countOfPlacedPoints++;
                    SendEvent();
                }
            }
            if (_countOfPlacedPoints == _countOfSmoothPoints)
            {
                if (_currentLine.LinePointsCount > 2)
                {
                    int oldPositionsCount = _currentLine.LinePointsCount; // 3  ||  7

                    Vector3[] positions = new Vector3[_countOfSmoothPoints]; // len = 3  ||  len = 3

                    for (int i = 0; i < _countOfSmoothPoints; i++)
                    {
                        positions[i] = _currentLine.lineRenderer.GetPosition(oldPositionsCount - _countOfSmoothPoints + i);
                    }

                    Vector3[] newPositions = Curver.MakeSmoothCurve(positions, 2.0f); // len = 6  ||  len = 6

                    _currentLine.LinePointsCount = oldPositionsCount + newPositions.Length - positions.Length;// 3 + 6 - 3 = 6  ||  7 + 6 - 3 = 10

                    for (int i = 0; i < newPositions.Length; i++) // i = 3 - 3 / i < 6  ||  7 - 3 = 4 / i < 10
                    {
                        if (oldPositionsCount - _countOfSmoothPoints + i > _currentLine.points.Count - 1)
                        {
                            _currentLine.points.Add(newPositions[i]);
                        }
                        else
                        {
                            _currentLine.points[i] = newPositions[i];
                        }
                        _currentLine.lineRenderer.SetPosition(oldPositionsCount - _countOfSmoothPoints + i, newPositions[i]);
                    }
                    _countOfPlacedPoints = 2;
                    SendEvent();
                }
                else
                {
                    _countOfPlacedPoints = _currentLine.LinePointsCount;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _countOfPlacedPoints = 0;
            _currentLine = null;
        }
    }

    private void SendEvent()
    {
        Vector3[] positions = new Vector3[_currentLine.LinePointsCount];
        _currentLine.lineRenderer.GetPositions(positions);
        OnPointAdded?.Invoke(positions);
    }

    public void CreatePoint(Vector3 position)
    {
        CreatePoint(position, _currentLine);
    }

    public void CreatePoint(Vector3 position, Line line)
    {
        if (line.LinePointsCount == 1 && _reference != null)
        {
            line.lineRenderer.SetPosition(0, _reference.position);
        }
        line.points.Add(position);
        line.LinePointsCount = line.points.Count;
        line.lineRenderer.SetPosition(line.LinePointsCount - 1, position + Vector3.up * LineOffsetY);
    }

    public Line DrawLine(LineRenderer lineRenderer, Material material, float lineWidth, UnityAction callback = null)
    {
        Line line = CreateLine(material, lineWidth, false);
        StartCoroutine(AnimatesLineCreation(lineRenderer, line, callback));
        return line;
    }

    public void ClearLine()
    {
        _currentLine = null;
    }

    public Line CopyLine(LineRenderer lineRenderer, Material material, float lineWidth)
    {
        Line line = CreateLine(material, lineWidth, false);
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            CreatePoint(positions[i], line);
        }
        return line;
    }

    private IEnumerator AnimatesLineCreation(LineRenderer lineRenderer, Line line, UnityAction callback = null)
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            yield return null;
            CreatePoint(positions[i], line);
        }
        callback?.Invoke();
    }

    public Line CreateLine()
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed);
        _currentLine = line;
        return line;
    }

    public Line CreateLine(Material material, float lineWidth, bool doCurrent = true)
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(material, lineWidth, OffsetSpeed);
        if (doCurrent)
        {
            _currentLine = line;
        }
        return line;
    }

    public Line CreateLine(Transform reference, Color color, float yOffset = 0)
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //CreatePoint(firstPoint);
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed, color);
        _currentLine = line;
        _currentLine.YOffset = yOffset;
        _reference = reference;
        return line;
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
    }*/
}

/*

public class Line
{
    public LineRenderer lineRenderer;
    public List<Vector3> points;

    private Material _material;
    private float _offsetSpeed;

    public int LinePointsCount
    {
        get
        {
            return lineRenderer.positionCount;
        }
        set
        {
            lineRenderer.positionCount = value;
        }
    }

    public void CutFromStart()
    {
        if (LinePointsCount < 2) return;
        Vector3[] positions = new Vector3[LinePointsCount];
        lineRenderer.GetPositions(positions);

        float Dist = Vector3.Distance(positions[0], positions[1]);
        points.Clear();
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 pos = positions[i + 1];
            lineRenderer.SetPosition(i, positions[i + 1]);
            points.Add(pos);
        }

        LinePointsCount--;
        _material.mainTextureOffset += new Vector2(Dist * _offsetSpeed, 0);
    }

    public void CutFromSecond()
    {
        if (LinePointsCount < 3) return;
        Vector3[] positions = new Vector3[LinePointsCount];
        lineRenderer.GetPositions(positions);

        points.Clear();
        for (int i = 1; i < positions.Length - 1; i++)
        {
            Vector3 pos = positions[i + 1];
            lineRenderer.SetPosition(i, positions[i + 1]);
            points.Add(pos);
        }

        LinePointsCount--;
    }

    public void ChangeMaterialOffset(float value)
    {
        _material.mainTextureOffset += new Vector2(value, 0);
    }

    public Line(Material material, float width, float offsetSpeed)
    {
        _offsetSpeed = offsetSpeed;

        points = new List<Vector3>();

        GameObject go = new GameObject();

        go.transform.eulerAngles = new Vector3(90, 0, 0);

        lineRenderer = go.AddComponent<LineRenderer>();

        _material = new Material(material);
        _material.CopyPropertiesFromMaterial(material);

        lineRenderer.material = _material;

        lineRenderer.textureMode = LineTextureMode.Tile;

        lineRenderer.positionCount = 0;

        lineRenderer.startWidth = width;

        lineRenderer.numCornerVertices = 6;

        lineRenderer.alignment = LineAlignment.TransformZ;
    }
}

-----------------------------------------------------------

    [SerializeField] private LayerMask Layers;
    [SerializeField] private float MinDistance = 0.1f;
    [SerializeField] private float LineOffsetY = .05f;
    [SerializeField] private float LineWidth = .05f;
    [SerializeField] private Material LineMaterial;
    [SerializeField] private float OffsetSpeed;

    private Camera _camera;

    private Vector3 _currentPosition;
    private Vector3 _lastPosition;

    //private List<Line> _lines;
    private Line _currentLine;

    private Transform _reference;

    private void Awake()
    {
        _camera = Camera.main;
        _lastPosition = Vector3.one * -100;
        //_lines = new List<Line>();
    }

    private const int _countOfSmoothPoints = 3;
    private int _countOfPlacedPoints = 0;

    private void Update()
    {
        if (_countOfPlacedPoints < _countOfSmoothPoints && _currentLine != null && Input.GetMouseButton(0))
        {
            if (RayHit(out RaycastHit hit))
            {
                if (!hit.point.DistanceLower(_lastPosition, MinDistance))
                {
                    _currentPosition = hit.point;
                    _lastPosition = _currentPosition;
                    CreatePoint(_currentPosition);
                    _countOfPlacedPoints++;
                }
            }
            if (_countOfPlacedPoints == _countOfSmoothPoints)
            {
                if (_currentLine.LinePointsCount > 2)
                {
                    int oldPositionsCount = _currentLine.LinePointsCount; // 3  ||  7

                    Vector3[] positions = new Vector3[_countOfSmoothPoints]; // len = 3  ||  len = 3

                    for (int i = 0; i < _countOfSmoothPoints; i++)
                    {
                        positions[i] = _currentLine.lineRenderer.GetPosition(oldPositionsCount - _countOfSmoothPoints + i);
                    }

                    Vector3[] newPositions = Curver.MakeSmoothCurve(positions, 2.0f); // len = 6  ||  len = 6

                    _currentLine.LinePointsCount = oldPositionsCount + newPositions.Length - positions.Length;// 3 + 6 - 3 = 6  ||  7 + 6 - 3 = 10

                    for (int i = 0; i < newPositions.Length; i++) // i = 3 - 3 / i < 6  ||  7 - 3 = 4 / i < 10
                    {
                        if (oldPositionsCount - _countOfSmoothPoints + i > _currentLine.points.Count - 1)
                        {
                            _currentLine.points.Add(newPositions[i]);
                        }
                        else
                        {
                            _currentLine.points[i] = newPositions[i];
                        }
                        _currentLine.lineRenderer.SetPosition(oldPositionsCount - _countOfSmoothPoints + i, newPositions[i]);
                    }
                    _countOfPlacedPoints = 2;
                }
                else
                {
                    _countOfPlacedPoints = _currentLine.LinePointsCount;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _countOfPlacedPoints = 0;
            _currentLine = null;
        }
    }

    public void CreatePoint(Vector3 position)
    {
        if (_currentLine.LinePointsCount == 1)
        {
            _currentLine.lineRenderer.SetPosition(0, _reference.position);
        }
        _currentLine.points.Add(position);
        _currentLine.LinePointsCount = _currentLine.points.Count;
        _currentLine.lineRenderer.SetPosition(_currentLine.LinePointsCount - 1, position + Vector3.up * LineOffsetY);
    }

    public Line CreateLine()
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed);
        _currentLine = line;
        return line;
    }

    public Line CreateLine(Transform reference)
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //CreatePoint(firstPoint);
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed);
        _currentLine = line;
        _reference = reference;
        return line;
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
 */

/**
 *
 *
    [SerializeField] private LayerMask Layers;
    [SerializeField] private float MinDistance = 0.1f;
    [SerializeField] private float LineOffsetY = .05f;
    [SerializeField] private float LineWidth = .05f;
    [SerializeField] private Material LineMaterial;
    [SerializeField] private float OffsetSpeed;
    [SerializeField] private bool Debug;

    private Camera _camera;

    private Vector3 _currentPosition;
    private Vector3 _lastPosition;

    //private List<Line> _lines;
    private Line _currentLine;

    private Transform _reference;

    public event UnityAction<Vector3[]> OnPointAdded;

    private void Awake()
    {
        _camera = Camera.main;
        _lastPosition = Vector3.one * -100;
        //_lines = new List<Line>();
    }

    private const int _countOfSmoothPoints = 3;
    private int _countOfPlacedPoints = 0;

    private void Update()
    {
        if (Debug && Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (_countOfPlacedPoints < _countOfSmoothPoints && _currentLine != null && _currentLine.lineRenderer != null && Input.GetMouseButton(0))
        {
            if (RayHit(out RaycastHit hit))
            {
                if (!hit.point.DistanceLower(_lastPosition, MinDistance))
                {
                    _currentPosition = hit.point + hit.normal * _currentLine.YOffset;
                    _lastPosition = _currentPosition;
                    CreatePoint(_currentPosition);
                    _countOfPlacedPoints++;
                    SendEvent();
                }
            }
            if (_countOfPlacedPoints == _countOfSmoothPoints)
            {
                if (_currentLine.LinePointsCount > 2)
                {
                    int oldPositionsCount = _currentLine.LinePointsCount; // 3  ||  7

                    Vector3[] positions = new Vector3[_countOfSmoothPoints]; // len = 3  ||  len = 3

                    for (int i = 0; i < _countOfSmoothPoints; i++)
                    {
                        positions[i] = _currentLine.lineRenderer.GetPosition(oldPositionsCount - _countOfSmoothPoints + i);
                    }

                    Vector3[] newPositions = Curver.MakeSmoothCurve(positions, 2.0f); // len = 6  ||  len = 6

                    _currentLine.LinePointsCount = oldPositionsCount + newPositions.Length - positions.Length;// 3 + 6 - 3 = 6  ||  7 + 6 - 3 = 10

                    for (int i = 0; i < newPositions.Length; i++) // i = 3 - 3 / i < 6  ||  7 - 3 = 4 / i < 10
                    {
                        if (oldPositionsCount - _countOfSmoothPoints + i > _currentLine.points.Count - 1)
                        {
                            _currentLine.points.Add(newPositions[i]);
                        }
                        else
                        {
                            _currentLine.points[i] = newPositions[i];
                        }
                        _currentLine.lineRenderer.SetPosition(oldPositionsCount - _countOfSmoothPoints + i, newPositions[i]);
                    }
                    _countOfPlacedPoints = 2;
                    SendEvent();
                }
                else
                {
                    _countOfPlacedPoints = _currentLine.LinePointsCount;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _countOfPlacedPoints = 0;
            _currentLine = null;
        }
    }

    private void SendEvent()
    {
        Vector3[] positions = new Vector3[_currentLine.LinePointsCount];
        _currentLine.lineRenderer.GetPositions(positions);
        OnPointAdded?.Invoke(positions);
    }

    public void CreatePoint(Vector3 position)
    {
        CreatePoint(position, _currentLine);
    }

    public void CreatePoint(Vector3 position, Line line)
    {
        if (line.LinePointsCount == 1 && _reference != null)
        {
            line.lineRenderer.SetPosition(0, _reference.position);
        }
        line.points.Add(position);
        line.LinePointsCount = line.points.Count;
        line.lineRenderer.SetPosition(line.LinePointsCount - 1, position + Vector3.up * LineOffsetY);
    }

    public Line DrawLine(LineRenderer lineRenderer, Material material, float lineWidth, UnityAction callback = null)
    {
        Line line = CreateLine(material, lineWidth, false);
        StartCoroutine(AnimatesLineCreation(lineRenderer, line, callback));
        return line;
    }

    public void ClearLine()
    {
        _currentLine = null;
    }

    public Line CopyLine(LineRenderer lineRenderer, Material material, float lineWidth)
    {
        Line line = CreateLine(material, lineWidth, false);
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            CreatePoint(positions[i], line);
        }
        return line;
    }

    private IEnumerator AnimatesLineCreation(LineRenderer lineRenderer, Line line, UnityAction callback = null)
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            yield return null;
            CreatePoint(positions[i], line);
        }
        callback?.Invoke();
    }

    public Line CreateLine()
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed);
        _currentLine = line;
        return line;
    }

    public Line CreateLine(Material material, float lineWidth, bool doCurrent = true)
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //return _lines[_lines.Count - 1];
        Line line = new Line(material, lineWidth, OffsetSpeed);
        if (doCurrent)
        {
            _currentLine = line;
        }
        return line;
    }

    public Line CreateLine(Transform reference, Color color, float yOffset = 0)
    {
        //_lines.Add(new Line(LineMaterial, LineWidth, OffsetSpeed));
        //_currentLine = _lines[_lines.Count - 1];
        //CreatePoint(firstPoint);
        //return _lines[_lines.Count - 1];
        Line line = new Line(LineMaterial, LineWidth, OffsetSpeed, color);
        _currentLine = line;
        _currentLine.YOffset = yOffset;
        _reference = reference;
        return line;
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
 */

/*
 *
 *
        if (DebugDrawing && Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (_countOfPlacedPoints < _countOfSmoothPoints && _currentLine != null && _currentLine.lineRenderer != null)
        {
            Vector3 position = Vector3.zero;
            if (Input.GetMouseButtonUp(0))
            {
                _lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0) && RayHit(out RaycastHit hit))
            {
                position = _lastFactPosition + hit.point;
            }
            else if (RayHit(_lastMousePosition, out RaycastHit hitAlone))
            {
                position = hitAlone.point;
                _lastFactPosition = position;
            }
            if (position != Vector3.zero && !position.DistanceLower(_lastPosition, MinDistance))
            {
                _currentPosition = position + Vector3.up * _currentLine.YOffset;
                _lastPosition = _currentPosition;
                CreatePoint(_currentPosition);
                _countOfPlacedPoints++;
                SendEvent();
            }
            if (_countOfPlacedPoints == _countOfSmoothPoints)
            {
                if (_currentLine.LinePointsCount > 2)
                {
                    int oldPositionsCount = _currentLine.LinePointsCount; // 3  ||  7

                    Vector3[] positions = new Vector3[_countOfSmoothPoints]; // len = 3  ||  len = 3

                    for (int i = 0; i < _countOfSmoothPoints; i++)
                    {
                        positions[i] = _currentLine.lineRenderer.GetPosition(oldPositionsCount - _countOfSmoothPoints + i);
                    }

                    Vector3[] newPositions = Curver.MakeSmoothCurve(positions, 2.0f); // len = 6  ||  len = 6

                    _currentLine.LinePointsCount = oldPositionsCount + newPositions.Length - positions.Length;// 3 + 6 - 3 = 6  ||  7 + 6 - 3 = 10

                    for (int i = 0; i < newPositions.Length; i++) // i = 3 - 3 / i < 6  ||  7 - 3 = 4 / i < 10
                    {
                        if (oldPositionsCount - _countOfSmoothPoints + i > _currentLine.points.Count - 1)
                        {
                            _currentLine.points.Add(newPositions[i]);
                        }
                        else
                        {
                            _currentLine.points[i] = newPositions[i];
                        }
                        _currentLine.lineRenderer.SetPosition(oldPositionsCount - _countOfSmoothPoints + i, newPositions[i]);
                    }
                    _countOfPlacedPoints = 2;
                    SendEvent();
                }
                else
                {
                    _countOfPlacedPoints = _currentLine.LinePointsCount;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _countOfPlacedPoints = 0;
            _currentLine = null;
        }
 */