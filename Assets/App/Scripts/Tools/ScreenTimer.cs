using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTimer : Singleton<ScreenTimer>
{
    private TextMeshProUGUI _text;
    [SerializeField] private GameObject LineKeeper;
    [SerializeField] private Transform Line;
    [SerializeField] private bool useLine = true;

    private void Start()
    {
        GetText();
    }

    private void GetText()
    {
        if (useLine) return;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = "";
    }

    public void Launch(int time, UnityAction callback)
    {
        if (!useLine)
        {
            if (_text == null)
            {
                GetText();
            }
            StartCoroutine(Count(time, callback));
        }
        else
        {
            Line.transform.localScale = Vector3.one;
            LineKeeper.gameObject.SetActive(true);
            Line.DOScaleX(0, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                callback?.Invoke();
                LineKeeper.gameObject.SetActive(false);
            });
        }
    }

    private IEnumerator Count(int time, UnityAction callback)
    {
        int currentTime = time;
        _text.text = time.ToString();
        WaitForSeconds delay = new WaitForSeconds(1f);
        for (int i = 0; i < time; i++)
        {
            currentTime--;
            yield return delay;
            _text.text = currentTime.ToString();
            _text.transform.DOScale(1.2f, .4f).SetEase(Ease.InOutExpo).OnComplete(() => _text.transform.DOScale(1f, .4f).SetEase(Ease.InOutExpo));
        }
        _text.text = "";
        callback?.Invoke();
    }
}