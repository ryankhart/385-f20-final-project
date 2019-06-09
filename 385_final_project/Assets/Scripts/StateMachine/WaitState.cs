using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    TownFolkAI owner;

    public WaitState(TownFolkAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Wait State");
    }

    public void Execute()
    {
        Debug.Log("Waiting Unit");
    }

    public void Exit()
    {
        Debug.Log("exiting wait state");
    }
}