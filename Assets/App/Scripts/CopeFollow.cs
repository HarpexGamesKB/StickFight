using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopeFollow : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectMovers;
    [SerializeField] private LineRenderer TargetPath;
    [SerializeField] private Material Material;
    [SerializeField] private float LineWidth = 0.25f;
    [SerializeField] private float Speed = 3f;
    [SerializeField] private float AngularSpeed = 10f;
    [SerializeField] private float MinimalDistance = .1f;
    [SerializeField] private float ActivateDelay = 1f;
    [SerializeField] private float AppearDelay = 1f;

    private Line[] _lines;

    private Transform[] _objectMoversInstance;

    private bool _launched;

    private void Start()
    {
        _lines = new Line[ObjectMovers.Length];
        for (int i = 0; i < ObjectMovers.Length - 1; i++)
        {
            _lines[i] = PathDrawer.Instance.CopyLine(TargetPath, Material, 0);
        }
        TargetPath.gameObject.SetActive(false);
        StartCoroutine(LaunchMovers());
    }

    private IEnumerator LaunchMovers()
    {
        yield return new WaitForSeconds(ActivateDelay);
        _lines[ObjectMovers.Length - 1] = PathDrawer.Instance.DrawLine(TargetPath, Material, LineWidth);
        //_lines[ObjectMovers.Length - 1] = PathDrawer.Instance.CopyLine(TargetPath, Material, LineWidth);
        yield return new WaitForSeconds(2f);
        _objectMoversInstance = new Transform[ObjectMovers.Length];
        for (int i = 0; i < ObjectMovers.Length; i++)
        {
            _objectMoversInstance[i] = Instantiate(ObjectMovers[i], _lines[i].lineRenderer.GetPosition(0), Quaternion.identity).transform;
            yield return new WaitForSeconds(AppearDelay);
        }
        _launched = true;
    }

    private void FixedUpdate()
    {
        CheckNull();
        if (_objectMoversInstance == null) return;
        for (int i = 0; i < _objectMoversInstance.Length; i++)
        {
            if (_lines[i] != null && _lines[i].LinePointsCount > 1)
            {
                if (_objectMoversInstance[i] != null)
                {
                    MoveByLine(_objectMoversInstance[i], ref _lines[i]);
                }
            }
        }
    }

    private void CheckNull()
    {
        if (_objectMoversInstance == null || !_launched) return;
        for (int i = 0; i < _objectMoversInstance.Length; i++)
        {
            if (_objectMoversInstance[i] != null)
            {
                return;
            }
        }
        Destroy(gameObject);
    }

    private void MoveByLine(Transform target, ref Line line)
    {
        if (line.LinePointsCount == 2)
        {
            Destroy(target.gameObject);
            return;
        }

        line.lineRenderer.SetPosition(0, target.position);

        float Dist = Vector3.Distance(line.lineRenderer.GetPosition(0), line.lineRenderer.GetPosition(1));

        Vector3 direction = (line.lineRenderer.GetPosition(1) - line.lineRenderer.GetPosition(0)).normalized;
        target.rotation = Quaternion.Slerp(target.rotation,
            Quaternion.AngleAxis(
                Quaternion.LookRotation(
                    direction
                    ).eulerAngles.y,
                Vector3.up)
            , AngularSpeed * Time.fixedDeltaTime);

        target.position += direction * Speed * Time.fixedDeltaTime;

        if (Dist <= MinimalDistance)
        {
            line.CutFromSecond();
        }
    }
}