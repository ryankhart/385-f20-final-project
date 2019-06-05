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
        //Debug.Log("Entering Searching State");
        owner.state = "Search";
    }

    public void Execute()
    {
        //Debug.Log("Searching");
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
                //if still null just wait;
            }
        }
    }

    public void Exit()
    {
        //Debug.Log("exiting Moving state");
    }
}
