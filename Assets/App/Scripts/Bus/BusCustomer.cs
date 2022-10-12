using DG.Tweening;
using UnityEngine;

public class BusCustomer : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void Appear()
    {
        transform.DOScale(1, 1f).SetEase(Ease.InOutExpo);
    }
}