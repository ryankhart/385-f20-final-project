using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherState : State
{

    TownFolkAI owner;

    public GatherState(TownFolkAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Gather State");
        owner.state = "Gather";
    }

    public void Execute()
    {
        owner.checkAction();
    }

    public void Exit()
    {
        Debug.Log("exiting Gather state");
    }
}
