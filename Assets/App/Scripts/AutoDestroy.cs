using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public bool collide = false;

    public void ToDestory(float delay)
    {
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutExpo).SetDelay(delay).OnComplete(() => Destroy(gameObject));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collide && collision.gameObject.layer == 6)
        {
            ToDestory(.5f);
        }
    }
}