using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStateMonster : IState
{
    MonsterAI owner;

    public WaitStateMonster(MonsterAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering wait State");
        owner.state = "Wait";
    }

    public void Execute()
    {
        if(owner.waitTownFolk(3))
        {
            owner.stateMachine.ChangeState(new SearchStateMonster(owner));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Wait state");
    }
}
