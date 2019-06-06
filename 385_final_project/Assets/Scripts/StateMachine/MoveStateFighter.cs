using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateFighter : IState
{
    FighterAI owner;

    public MoveStateFighter(FighterAI owner)
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
                owner.stateMachine.ChangeState(new GatherStateFighter(owner));
            }
            else
            {
                owner.nodesOfMovement++;
                owner.FindNode();
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
