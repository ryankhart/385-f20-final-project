using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMonster : State
{
    MonsterAI owner;

    public AttackStateMonster(MonsterAI owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        Debug.Log("Entering Attack State");
        owner.state = "Attack";
    }

    public void Execute()
    {
        owner.targetObject.GetComponent<TownFolkAI>().stateMachine.ChangeState(new FleeState(owner.targetObject.GetComponent<TownFolkAI>()));
        owner.stateMachine.ChangeState(new WaitStateMonster(owner));
    }

    public void Exit()
    {
        Debug.Log("Exiting Attack state");
    }
}