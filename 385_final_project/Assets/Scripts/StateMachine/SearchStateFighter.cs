using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateFighter : IState
{
    FighterAI owner;

    public SearchStateFighter(FighterAI owner)
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
        if (owner.targetObject != null)
        {
            owner.stateMachine.ChangeState(new MoveStateFighter(owner));
        }
        else
        {
            owner.setTag("Fort");
            if (owner.targetObject != null)
            {
                //if still null just wait;
            }
        }
    }

    public void Exit()
    {
        Debug.Log("exiting Moving state");
    }
}
