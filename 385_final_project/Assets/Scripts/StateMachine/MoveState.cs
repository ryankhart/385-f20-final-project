using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
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
                if (owner.nodesOfMovement > 5)
                {
                    owner.stateMachine.ChangeState(new SearchState(owner));
                }
            }

        }
        else
        {
            owner.moveToDestination();
        }
    }

    public void Exit()
    {
    }
}
