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
        Debug.Log("Entering Searching State");
        owner.state = "Search";
    }

    public void Execute()
    {
        Debug.Log("Searching");
        owner.FindPath();
        owner.FindNode();
        owner.stateMachine.ChangeState(new MoveState(owner));
    }

    public void Exit()
    {
        Debug.Log("exiting Moving state");
    }
}
