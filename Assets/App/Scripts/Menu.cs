using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField] private RectTransform UpgradeMenuPlane;
    [SerializeField] private RectTransform PlayButtonPlane;

    [Header("Params")]
    [SerializeField] private float UpgradeMenuSlideDuration;

    [SerializeField] private float PlayButtonSlideDuration;

    private void Start()
    {
        Open();
    }

    public void Open()
    {
        UpgradeMenuPlane.DOAnchorPosX(0, UpgradeMenuSlideDuration).SetEase(Ease.InOutExpo).SetDelay(.25f);
        PlayButtonPlane.DOAnchorPosX(0, PlayButtonSlideDuration).SetEase(Ease.InOutExpo).SetDelay(.35f);
    }

    public void Hide()
    {
        UpgradeMenuPlane.DOAnchorPosX(-1080, UpgradeMenuSlideDuration).SetEase(Ease.InOutExpo);
        PlayButtonPlane.DOAnchorPosX(-1080, PlayButtonSlideDuration).SetEase(Ease.InOutExpo).SetDelay(.1f);
    }
}