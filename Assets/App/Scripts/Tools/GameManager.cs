using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public event UnityAction OnWon;
    public event UnityAction OnLost;
    public event UnityAction<int> OnLevelRestarted;
    public event UnityAction<int> OnLevelStarted;

    public bool CanInterract { private set; get; }

    public void LevelWin()
    {
        OnWon?.Invoke();
    }

    public void LevelLose()
    {
        OnLost?.Invoke();
    }

    public void LevelStart(int levelId)
    {
        OnLevelStarted?.Invoke(levelId);
    }

    public void LevelRestart(int levelId)
    {
        OnLevelRestarted?.Invoke(levelId);
    }
}
