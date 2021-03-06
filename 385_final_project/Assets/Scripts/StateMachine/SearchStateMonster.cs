﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateMonster : State
{
    MonsterAI owner;

    public SearchStateMonster(MonsterAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Searching State");
        owner.state = "Search";
    }

    public void Execute()
    {
        Debug.Log("Searching");
        owner.FindPath();
        owner.FindNode();
        //Should stop the waiting
        if(owner.pathArray.Count > 0 && owner.targetObject != null)
        {
            owner.stateMachine.ChangeState(new MoveStateMonster(owner));
        }
    }

    public void Exit()
    {
        Debug.Log("exiting Moving state");
    }
}