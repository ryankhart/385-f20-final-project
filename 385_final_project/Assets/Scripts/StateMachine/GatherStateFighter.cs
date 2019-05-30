using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherStateFighter : IState
{

    FighterAI owner;

    public GatherStateFighter(FighterAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Fighting State");
        owner.state = "Fight";
    }

    public void Execute()
    {
        owner.checkAction();
    }

    public void Exit()
    {
        Debug.Log("exiting Fighting state");
    }
}