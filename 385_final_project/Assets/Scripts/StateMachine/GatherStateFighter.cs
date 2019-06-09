using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherStateFighter : State
{

    FighterAI owner;

    public GatherStateFighter(FighterAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
       
        owner.state = "Fight";
    }

    public void Execute()
    {
        owner.checkAction();
    }

    public void Exit()
    {
    }
}