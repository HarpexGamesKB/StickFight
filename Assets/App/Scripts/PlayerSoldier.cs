using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoldier : ObjectMover
{
    public override void Update()
    {
        if (CanRun)
        {
            MoveByLineForPlayers();
        }
        
        //base.Update();
    }
    private void MoveByLineForPlayers()
    {
        animator.SetBool("IsRun", true);
        if (Agent.enabled == true)
        {
            Agent.SetDestination(PlayersPositions[numberInGroup].position);
        }
        if (Vector3.Distance(transform.position, PlayersPositions[numberInGroup].position) < MinimalDistance && MainPlayer.CanRun == false)
        {
            Agent.stoppingDistance = 4f;
            animator.SetBool("IsRun", false);
            CanRun = false;
        }
    }
    public override void OnDamageTaken()
    {
        MainPlayer.numberOfDamagedPlayers++;
        Debug.Log("numberOfDamagedPlayers " + MainPlayer.numberOfDamagedPlayers);
        PlayerPrefs.SetInt("numberOfDamagedPlayers", MainPlayer.numberOfDamagedPlayers);
    }
    public override void OnDead()
    {
        base.OnDead();
        MainPlayer.Players.Remove(this);
    }
    public override void Start()
    {
        
    }
    public override void MoveCamera()
    {
        
    }
}
