using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameState previousState;

    protected Character character;

    public GameState(Character character)
    {
        this.character = character;
    }

    public virtual void EnterState()
    {
        //check if can execute state

        ExecuteState();
    }

    public abstract void ExecuteState();

    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState()
    {
        previousState = this;
    }
}
