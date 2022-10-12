using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    [SerializeField] private Transform AnimatedObject;
    [SerializeField] private Renderer LineRenderer;
    [SerializeField] private Renderer ShineRenderer;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private bool HideAfterFill;
    private Material _ownMaterial;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 10, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
        Colorize();
        ShineRenderer.material.DOFade(.2f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnValidate()
    {
        Colorize();
    }

    private void Reset()
    {
        Colorize();
    }

    private void Colorize()
    {
        if (_ownMaterial == null)
        {
            _ownMaterial = new Material(GetComponent<MaterialKeeper>().Material);
        }
        if (LineRenderer.sharedMaterial != _ownMaterial)
        {
            LineRenderer.sharedMaterial = _ownMaterial;
            LineRenderer.sharedMaterial.color = color1;
        }
    }

    public void Fill(GameObject Object)
    {
        if (HideAfterFill)
        {
            GetComponent<Collider>().enabled = false;
            transform.DOScale(0, 1f).SetEase(Ease.InOutExpo);
        }
        MoveObjectGenerator.Instance.Reduce(Object);

        /*
         * AnimatedObject.DOScale(1.2f, inDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            AnimatedObject.DOScale(1f, outDuration).SetEase(Ease.InOutSine);
        });
        */

        LineRenderer.material.DOColor(color2, .25f).OnComplete(() => LineRenderer.material.DOColor(color1, .25f));
    }
}