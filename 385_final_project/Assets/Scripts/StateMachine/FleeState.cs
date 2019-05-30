using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : IState
{
    TownFolkAI owner;

    public FleeState(TownFolkAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Flee State");
        owner.state = "Flee";
    }

    public void Execute()
    {
        owner.setTag("Home");
        owner.inventory = 0;
        owner.stateMachine.ChangeState(new SearchState(owner));
        owner.stateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("exiting Flee state");
    }
}