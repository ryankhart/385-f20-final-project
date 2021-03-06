﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStateMonster : State
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
        if(owner.waitTownFolk(1))
        {
            owner.stateMachine.ChangeState(new SearchStateMonster(owner));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Wait state");
    }
}
