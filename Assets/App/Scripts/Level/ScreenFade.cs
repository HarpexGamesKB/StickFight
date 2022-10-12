using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenFade : Singleton<ScreenFade>
{
    [SerializeField] private Image Image;

    private void Start()
    {
        FadeOut();
    }

    public void FadeIn(UnityAction callback = null)
    {
        Image.raycastTarget = true;
        Image.DOFade(1f, .5f).SetEase(Ease.InOutExpo).OnComplete(() => callback?.Invoke());
    }

    public void FadeOut(UnityAction callback = null)
    {
        Image.DOFade(0f, .5f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            callback?.Invoke();
            Image.raycastTarget = false;
        });
    }
}