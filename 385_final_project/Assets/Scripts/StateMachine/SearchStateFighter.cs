using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateFighter : State
{
    FighterAI owner;

    public SearchStateFighter(FighterAI owner)
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
        if (owner.targetObject != null && owner.pathArray.Count > 0)
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
    }
}
