using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutscreenIndicatorManager : Singleton<OutscreenIndicatorManager>
{
    [SerializeField] private OutscreenIndicatorPresenter IndicatorPrefab;
    [SerializeField] private Transform Parent;
    private Game.Pooler _indicatorPool;

    private void Start()
    {
        _indicatorPool = new Game.Pooler(IndicatorPrefab, Parent);
    }

    public OutscreenIndicatorPresenter GetIndicator()
    {
        return _indicatorPool.GetObject() as OutscreenIndicatorPresenter;
    }
}