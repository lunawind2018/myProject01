using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleStateMachine {

    public int State { get; protected set; }

    public virtual bool Switch(int newState)
    {
        return true;
    }

    protected virtual bool OnEnterState()
    {
        return true;
    }

    protected virtual bool OnExitState()
    {
        return true;
    }

}
