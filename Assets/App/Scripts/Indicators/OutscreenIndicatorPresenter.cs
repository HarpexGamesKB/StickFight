using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OutscreenIndicatorPresenter : MonoBehaviour
{
    [SerializeField] private Image IndicatorIcon;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        transform.DOScale(Vector3.one * .9f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetIcon(Sprite sprite)
    {
        IndicatorIcon.sprite = sprite;
    }

    public Vector2 position
    {
        get
        {
            return _rect.anchoredPosition;
        }
        set
        {
            _rect.anchoredPosition = value;
        }
    }
}