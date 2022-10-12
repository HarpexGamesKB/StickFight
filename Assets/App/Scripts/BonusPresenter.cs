using DG.Tweening;
using TMPro;
using UnityEngine;

public class BonusPresenter : MonoBehaviour
{
    [SerializeField] private Transform Plane;
    [SerializeField] private TextMeshProUGUI Text;

    public void Present(int x)
    {
        Text.text = Economics.Instance.levelCoins + "x" + x + "=" + Economics.Instance.levelCoins * x;
        Plane.DOScale(Vector3.one, .8f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            Plane.DOScale(Vector3.zero, .8f).SetEase(Ease.InOutExpo).SetDelay(2f).OnComplete(() =>
            {
                Economics.Instance.AddCoins(Economics.Instance.levelCoins * x - Economics.Instance.levelCoins);
                OSLevelManager.Instance.NextLevel(0, false);
            });
        });
    }
}