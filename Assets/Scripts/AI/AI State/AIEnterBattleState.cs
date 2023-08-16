using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnterBattleState : AIState
{
    private bool canAttack;

    public AIEnterBattleState(AI aI) : base(aI)
    {
    }

    public override void EnterState()
    {
        if (!BattleSystem.Instance.firstTurn && BattleState.Instance.CanChangeToBattleState())
        {
            canAttack = true;

            base.EnterState();
        }

        else
        {
            canAttack = false;

            aI.ChangeState(aI.GetAIState(AI.State.AIEndTurnState));
        }
    }

    public override IEnumerator ExecuteState()
    {
        if (canAttack)
        {
            aI.StartCoroutine(PhaseManager.Instance.ChangePhase(PhaseManager.Phase.Battle));

            aI.ChangeState(aI.GetAIState(AI.State.AIAttackState));
        }

        yield return null;
    }

    public override void ExitState()
    {
        base.ExitState();

        canAttack = false;
    }
}
