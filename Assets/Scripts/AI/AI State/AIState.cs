using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected static AIState previousState;

    protected AI aI;

    protected float TIME_DELAY_BETWEEN_STATE = 1.5f;

    protected bool isDelaying;

    protected bool lockState;

    public AIState(AI aI)
    {
        this.aI = aI;
    }

    public virtual void EnterState()
    {
        //check if can execute state

        if (lockState)
        {
            return;
        }

        isDelaying = true;

        aI.StartCoroutine(AIDelayAndExecuteState());
    }

    public abstract IEnumerator ExecuteState();

    public virtual void UpdateState()
    {
        if (isDelaying)
        {
            return;
        }
    }

    public virtual void ExitState()
    {
        if (isDelaying)
        {
            isDelaying = false;

            aI.StopCoroutine(AIDelayAndExecuteState());
        }

        previousState = this;
    }

    public IEnumerator AIDelayAndExecuteState()
    {
        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_STATE);

        isDelaying = false;

        yield return aI.StartCoroutine(ExecuteState());
    }

    public IEnumerator AIDelay()
    {
        isDelaying = true;

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_STATE);

        isDelaying = false;
    }
}
