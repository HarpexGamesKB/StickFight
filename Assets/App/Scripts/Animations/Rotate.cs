using DG.Tweening;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField] private int Side = 1;

    [SerializeField] private float Duration = 3f;
    private Tween _tween;

    private void Start()
    {
        _tween = transform.DOLocalRotate(Vector3.up * 360 * Side, Duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}