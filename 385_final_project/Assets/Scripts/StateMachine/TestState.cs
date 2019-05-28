using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : IState
{
    Unit owner;

    public TestState(Unit owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("entering test state");
    }

    public void Execute()
    {
        Debug.Log("updating test state");
    }

    public void Exit()
    {
        Debug.Log("exiting test state");
    }
}


