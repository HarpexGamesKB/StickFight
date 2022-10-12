using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    [SerializeField] private LayerMask Layer;

    [SerializeField] private float Radius;
    [SerializeField] private Transform Circle;
    [SerializeField] private Transform Sphere;

    private bool _dangerExist;

    private void Start()
    {
        StartCoroutine(Search());
    }

    private IEnumerator Search()
    {
        WaitForSeconds delay = new WaitForSeconds(.1f);
        while (enabled)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, Layer);
            if (colliders.Length > 1)
            {
                Enable();
            }
            else
            {
                Disable();
            }
            yield return delay;
        }
    }

    private void Enable()
    {
        if (_dangerExist) return;
        _dangerExist = true;

        Circle.DOKill();
        Circle.DOScale(Vector3.one * (Radius + 1), 1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Circle.transform.localScale = Vector3.zero;
        });
        Sphere.DOScale(Vector3.one * 2.5f, 1f).SetEase(Ease.OutSine);
    }

    private void Disable()
    {
        if (!_dangerExist) return;
        _dangerExist = false;
        Circle.DOKill();
        Sphere.DOScale(Vector3.zero, .5f).SetEase(Ease.OutExpo);
        Circle.transform.localScale = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Tools.DrawCircle(transform.position + Vector3.up * .1f, Radius);
    }
}