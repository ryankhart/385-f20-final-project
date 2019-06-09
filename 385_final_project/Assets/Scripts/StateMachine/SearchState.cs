using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : State
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
        if(owner.targetObject != null || owner.pathArray.Count > 0)
        {
           owner.stateMachine.ChangeState(new MoveState(owner));
        }
        else
        {
            //Fixes the waiting around if the resoruce isn't around;
            owner.setTag("Home");
            //If there isn't a home they will just wait?
        }
    }

    public void Exit()
    {
    }
}
