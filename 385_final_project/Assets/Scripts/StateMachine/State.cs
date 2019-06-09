using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    void Enter();
    void Execute();
    void Exit();
}