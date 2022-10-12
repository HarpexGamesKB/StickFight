using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BusTrigger : MonoBehaviour
{
    [SerializeField] private BusCustomer[] BusCustomers;

    private GameObject _finisher;

    public void Init()
    {
        //_finisher = finisher;
        //_finisher.SetActive(false);
        //_finisher.transform.localScale = Vector3.zero;
        transform.localScale = Vector3.zero;
        for (int i = 0; i < BusCustomers.Length; i++)
        {
            BusCustomers[i].transform.localScale = Vector3.zero;
        }
        Appear();
    }

    public void Appear()
    {
        transform.DOScale(1f, 1f).SetEase(Ease.InOutExpo);
        for (int i = 0; i < BusCustomers.Length; i++)
        {
            BusCustomers[i].transform.DOScale(1f, 1f).SetEase(Ease.InOutExpo);
        }
    }

    public void Fill(UnityAction callback)
    {
        StartCoroutine(_Fill(callback));
    }

    private IEnumerator _Fill(UnityAction callback)
    {
        for (int i = 0; i < BusCustomers.Length; i++)
        {
            Destroy(BusCustomers[i].gameObject);
            yield return new WaitForSeconds(.2f);
        }
        callback?.Invoke();
        //_finisher.SetActive(true);
        //_finisher.transform.DOScale(1f, 1f).SetEase(Ease.InOutExpo);
        transform.DOScale(0f, 1.05f).SetEase(Ease.InOutExpo).OnComplete(() => Destroy(gameObject));
    }
}