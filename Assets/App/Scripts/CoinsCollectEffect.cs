using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectEffect : Singleton<CoinsCollectEffect>
{
    [SerializeField] private RectTransform CoinImagePrefab;
    [SerializeField] private RectTransform TargetCoinImage;
    [SerializeField] private Transform Parent;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void Collect(Vector3 worldPosition)
    {
        Vector3 startPosition = _camera.WorldToScreenPoint(worldPosition);
        RectTransform coin = Instantiate(CoinImagePrefab, startPosition, Quaternion.identity, Parent);
        coin.localScale = Vector3.zero;
        coin.DOScale(Vector3.one, .4f).SetEase(Ease.InOutExpo);
        coin.DOAnchorPos(TargetCoinImage.position - Vector3.right * 75 / 2f - Vector3.up * 75 / 2.2f, .8f).SetEase(Ease.InOutExpo).OnComplete(() => Destroy(coin.gameObject));
    }
}