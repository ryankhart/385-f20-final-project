using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateMonster : State
{
    MonsterAI owner;

    public MoveStateMonster(MonsterAI owner)
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
                owner.stateMachine.ChangeState(new AttackStateMonster(owner));
            }
            else
            {
                owner.nodesOfMovement++;
                owner.FindNode();
                if(owner.nodesOfMovement > 3)
                {
                    owner.stateMachine.ChangeState(new SearchStateMonster(owner));
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
