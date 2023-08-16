using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitingState : AIState
{
    public AIWaitingState(AI aI) : base(aI)
    {
        //BattleState.Instance.OnAttackFinished += BattleState_OnAttackFinished;

        BattleSystem.Instance.OnActionsFinished += BattleSystem_OnActionsFinished;
    }

    private void BattleSystem_OnActionsFinished(object sender, Character.OnCharacterTriggerEventEventArgs e)
    {
        if (e.character == AI.Instance)
        {
            aI.ChangeState(previousState);
        }
    }

    private void BattleState_OnAttackFinished(object sender, System.EventArgs e)
    {
        if (aI.GetCurrentAIState() == aI.GetAIState(AI.State.AIWaitingState))
        {
            aI.ChangeState(aI.GetAIState(AI.State.AIAttackState));
        }
    }

    private void BattleSystem_OnEffectFinished(object sender, Character.OnCharacterTriggerEventEventArgs e)
    {
        if (aI.GetCurrentAIState() == aI.GetAIState(AI.State.AIWaitingState))
        {
            aI.ChangeState(aI.GetAIState(AI.State.AIEnterBattleState));
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Waiting...");
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // searching...

        return;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override IEnumerator ExecuteState()
    {
        yield return null;
    }
}
