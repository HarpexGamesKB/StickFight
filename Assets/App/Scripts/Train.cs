using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private RailwayIndicator RailwayIndicator;

    [Space]
    [SerializeField] private Transform TrainTransform;

    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform EndPoint;

    [SerializeField] private float Duration;
    [SerializeField] private float MinDelayTest;
    [SerializeField] private float MaxDelayTest;
    [SerializeField] private float ActivateDelay;

    private void Start()
    {
        TrainTransform.gameObject.SetActive(false);
        TrainTransform.transform.position = StartPoint.position;
        ActivateSystem(ActivateDelay);
    }

    public void ActivateSystem(float delay)
    {
        float random = Random.Range(MinDelayTest, MaxDelayTest);
        Invoke(nameof(AcitvateIndicator), delay + random - 1);
        Invoke(nameof(Activate), delay + random);
    }

    private void AcitvateIndicator()
    {
        RailwayIndicator.Enable();
    }

    private void Activate()
    {
        TrainTransform.DOMove(EndPoint.position, Duration)
            .SetEase(Ease.Linear)
            .OnStart(() => TrainTransform.gameObject.SetActive(true))
            .OnComplete(() =>
            {
                TrainTransform.gameObject.SetActive(false);
                TrainTransform.transform.position = StartPoint.position;
                ActivateSystem(0);
                RailwayIndicator.Disable();
            });
    }
}