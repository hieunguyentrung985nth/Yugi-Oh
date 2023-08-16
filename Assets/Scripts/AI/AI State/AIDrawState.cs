using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDrawState : AIState
{
    public AIDrawState(AI aI) : base(aI)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Entering Draw State");

        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Draw State");

        base.ExitState();
    }

    public override IEnumerator ExecuteState()
    {
        yield return null;
    }
}
