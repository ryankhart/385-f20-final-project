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
        owner.state = "Move";
    }

    public void Execute()
    {
        owner.moveToDestination();
        Vector3 villagerPosition = owner.transform.position;
        villagerPosition.y = 0;

        if (owner.checkIfAtNode(villagerPosition))
        {
            if (owner.checkIfAtDestination(villagerPosition))
            {
                owner.stateMachine.ChangeState(new GatherState(owner));
            }
            else
            {
                owner.nodesOfMovement++;
                owner.FindNode();
            }

        }
    }

    public void Exit()
    {
    }
}
