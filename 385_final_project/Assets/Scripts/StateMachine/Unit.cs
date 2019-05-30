using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();

    void Start()
    {
        stateMachine.ChangeState(new TestState(this));
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void getSomething()
    {
        Debug.Log("GETTING SOMETHING");
    }
}
