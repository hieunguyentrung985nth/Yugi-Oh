using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(AI aI) : base(aI)
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        aI.ChangeState(this);

        lockState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");

        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();        
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");

        base.ExitState();
    }

    public override IEnumerator ExecuteState()
    {
        Debug.Log("Chilling...");

        yield return null;
    }
}
