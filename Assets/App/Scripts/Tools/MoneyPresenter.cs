using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyPresenter : EventBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        if (!Economics.Instance) return;
        _moneyText.text = Economics.Instance.coins.ToString();
    }

    protected override void RegisterEvents()
    {
        if (!Economics.Instance) return;
        Economics.Instance.OnCoinsUpdated += ChangeManeyText;
    }

    protected override void UnregisterEvents()
    {
        if (!Economics.Instance) return;
        Economics.Instance.OnCoinsUpdated -= ChangeManeyText;
    }

    private void ChangeManeyText(int count, bool animated)
    {
        _moneyText.text = count.ToString();
        if (animated)
        {
            _moneyText.transform.DOScale(1.2f, .2f).SetEase(Ease.InOutSine).OnComplete(() => _moneyText.transform.DOScale(1f, .5f).SetEase(Ease.InOutSine));
        }
    }
}