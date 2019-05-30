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
        Debug.Log("Entering Moving State");
        owner.state = "Move";
    }

    public void Execute()
    {
        Debug.Log("Moving Unit");
        owner.moveToDestination();
        Vector3 villagerPosition = owner.transform.position;
        villagerPosition.y = 0;

        if(owner.nodesOfMovement > 30)
        {
            owner.stateMachine.ChangeState(new SearchStateFighter(owner));
        }



        if (owner.checkIfAtNode(villagerPosition))
        {
            Debug.Log("At Node!");
            if (owner.checkIfAtDestination(villagerPosition))
            {
                owner.stateMachine.ChangeState(new GatherStateFighter(owner));
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
