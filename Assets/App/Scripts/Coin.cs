using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private LayerMask Layer;
    [SerializeField] private GameObject Effect;

    private bool _triggered;

    private void Start()
    {
        transform.position += Vector3.up * .5f;
        gameObject.AddComponent<Rotate>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && Tools.HasLayer(Layer, other.gameObject.layer))
        {
            _triggered = true;
            Log._(Economics.Instance.DefaultCoinWeigth);
            Economics.Instance.AddCoins(Economics.Instance.DefaultCoinWeigth);
            CoinsCollectEffect.Instance.Collect(transform.position);
            if (Effect)
            {
                Instantiate(Effect, transform.position, Quaternion.identity);
            }
            transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}