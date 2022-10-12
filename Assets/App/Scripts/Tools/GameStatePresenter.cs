using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePresenter : EventBehaviour
{
    [SerializeField] private GameObject _winObject;
    [SerializeField] private GameObject _loseObject;
    [SerializeField] private GameObject _battleObject;

    private void WinPresenter()
    {
        _winObject.SetActive(true);
    }

    private void LosePresenter()
    {
        _loseObject.SetActive(true);
    }

    private void LevelRestarted(int index)
    {
        _loseObject.SetActive(false);
        _winObject.SetActive(false);
        _battleObject.SetActive(true);
    }

    private void LevelStarted(int index)
    {
        _battleObject.SetActive(false);
    }

    protected override void RegisterEvents()
    {
        GameManager.Instance.OnWon += WinPresenter;
        GameManager.Instance.OnLost += LosePresenter;
        GameManager.Instance.OnLevelRestarted += LevelRestarted;
        GameManager.Instance.OnLevelStarted += LevelStarted;
    }

    protected override void UnregisterEvents()
    {
        return;
        GameManager.Instance.OnWon -= WinPresenter;
        GameManager.Instance.OnLost -= LosePresenter;
        GameManager.Instance.OnLevelRestarted -= LevelRestarted;
    }
}
