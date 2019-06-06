using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IState
{
    TownFolkAI owner;

    public SearchState(TownFolkAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.state = "Search";
    }

    public void Execute()
    {
        owner.FindPath();
        owner.FindNode();
        if(owner.targetObject != null)
        {
           owner.stateMachine.ChangeState(new MoveState(owner));
        }
        else
        {
            owner.setTag("Home");
            if (owner.targetObject != null)
            {
                //Try again
                owner.stateMachine.ChangeState(new MoveState(owner));
            }
        }
    }

    public void Exit()
    {
    }
}
