using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEndTurnState : AIState
{
    public AIEndTurnState(AI aI) : base(aI)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Nothing to play.");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override IEnumerator ExecuteState()
    {
        Debug.Log("End Turn");

        aI.StartCoroutine(PhaseManager.Instance.ChangePhase(PhaseManager.Phase.End));

        aI.StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.EndTurn));

        yield return null;
    }
}
