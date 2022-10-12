using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : Enemy
{
    public override void Update()
    {
        if (CanRun)
        {
            MoveByLineForPlayers();
        }
    }
    private void MoveByLineForPlayers()
    {
        animator.SetBool("IsRun", true);
        if (Agent.enabled == true)
        {
            Agent.SetDestination(EnemiesPositions[numberInGroup].position);
        }
        if (Vector3.Distance(transform.position, EnemiesPositions[numberInGroup].position) < MinimalDistance && MainEnemy.CanRun == false)
        {
            Agent.stoppingDistance = 0f;
            animator.SetBool("IsRun", false);
        }

    }
    public override void OnDamageTaken()
    {
        MainEnemy.numberOfDamagedEnemies++;
        Debug.Log("numberOfDamagedEnemies " + MainEnemy.numberOfDamagedEnemies);
        PlayerPrefs.SetInt("numberOfDamagedEnemies", MainEnemy.numberOfDamagedEnemies);
    }
    public override void OnDead()
    {
        base.OnDead();
        MainEnemy.Enemies.Remove(this);
    }
    public override void Start()
    {

    }
}
