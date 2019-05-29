using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    TownFolkAI owner;

    public MoveState(TownFolkAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Moving State");
    }

    public void Execute()
    {
        Debug.Log("Moving Unit");
        owner.moveToDestination();
        Vector3 villagerPosition = owner.transform.position;
        villagerPosition.y = 0;

        if (owner.checkIfAtNode(villagerPosition))
        {
            Debug.Log("At Node!");
            if (owner.checkIfAtDestination(villagerPosition))
            {
                owner.stateMachine.ChangeState(new GatherState(owner));
            }
            else
            {
                Debug.Log("Node of Movement up by 1");
                owner.nodesOfMovement++;
                owner.FindNode();
            }

        }
        Debug.Log("Done moving!");
    }

    public void Exit()
    {
        Debug.Log("exiting Moving state");
    }
}
