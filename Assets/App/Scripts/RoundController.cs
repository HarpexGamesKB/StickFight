using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : Singleton<RoundController>
{
    public ObjectMover PlayerKing;
    public Transform Camera;
    public Enemy EnemyKing;
    private bool isGameOver;
    public void GameProcessChecker()
    {
        if (isGameOver) return;
        

        if(PlayerKing.Players.Count == 0 && PlayerKing.Health <=0)
        {
            isGameOver = true;
            GameOver();
            return;
        }else if(EnemyKing.Enemies.Count == 0 && EnemyKing.Health <= 0)
        {
            isGameOver = true;
            GameWin();
            return;
        }

    }
    private void Update()
    {
        if (isGameOver) return;
        if (PlayerKing.CanDraw == false && PlayerKing.CanRun == false && EnemyKing.CanRun == false)
        {

            StartCoroutine(ControlCheck());
            return;
            //дати з плеєрпрефс сигнал кінгам що це другий раунд і присвоїти всім відповідне здоров'я
        }
    }
    private IEnumerator ControlCheck()
    {
        yield return new WaitForSeconds(1);
        if(PlayerKing.CanDraw == false && PlayerKing.CanRun == false && EnemyKing.CanRun == false)
        {
            GameProcessChecker();
            if (isGameOver == false)
            {
                TryAgain();
            }
        }
    }
    public void GameOver()
    {
        OSLevelManager.Instance.TestLose();
        PlayerPrefs.SetInt("IsSecontRound", 0);
        SetDefaultParameters();
    }
    public void GameWin()
    {
        OSLevelManager.Instance.TestWin();
        PlayerPrefs.SetInt("IsSecontRound", 0);
        SetDefaultParameters();
    }
    public void TryAgain()
    {
        isGameOver = true;
        OSLevelManager.Instance.TestTryAgain();
        PlayerPrefs.SetInt("IsSecontRound",1);
        SaveParameters();
    }
    public void Restart()
    {
        OSLevelManager.Instance.RestartScene();
        SetDefaultParameters();
    }
    private void SaveParameters()
    {
        if (PlayerKing.Health <= 0)
        {
            PlayerPrefs.SetInt("PlayersCount", PlayerKing.Players.Count-1);
        }
        else
        {
            PlayerPrefs.SetInt("PlayersCount", PlayerKing.Players.Count);
        }
        if (EnemyKing.Health <= 0)
        {
            PlayerPrefs.SetInt("EnemiesCount", EnemyKing.Enemies.Count-1);
        }
        else
        {
            PlayerPrefs.SetInt("EnemiesCount", EnemyKing.Enemies.Count);
        }
    }
    private void SetDefaultParameters()
    {
        PlayerPrefs.SetInt("IsSecontRound", 0);
        PlayerPrefs.SetInt("PlayersCount", 0);
        PlayerPrefs.SetInt("EnemiesCount", 0);
        PlayerPrefs.SetInt("numberOfDamagedPlayers",0);
        PlayerPrefs.SetInt("numberOfDamagedEnemies", 0);
        PlayerPrefs.SetInt("MainPlayerDamaged", 0);
        PlayerPrefs.SetInt("MainEnemyDamaged", 0);
        PlayerPrefs.SetInt("PlayersBuyed", 0);
    }
    public void BuyPlayer()
    {
        int count = PlayerPrefs.GetInt("PlayersBuyed", 0);
        if (count <9)
        {
            PlayerPrefs.SetInt("PlayersBuyed", count+1);
            PlayerKing.AddStickmans(count + 1);
        }
    }
}
