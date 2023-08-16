using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AIChangePosition : AIState
{
    private MonsterCard monsterToChangePosition;

    public AIChangePosition(AI aI) : base(aI)
    {
    }

    public override void EnterState()
    {
        List<MonsterCard> monstersOnField = AI.Instance.GetMonsterZone().GetMonsterCardsOnField();

        foreach (MonsterCard monster in monstersOnField)
        {
            if (monster.CanChangePosition())
            {
                monsterToChangePosition = monster;

                base.EnterState();

                break;
            }
        }

        if (monsterToChangePosition == null)
        {
            monsterToChangePosition = null;

            aI.ChangeState(aI.GetAIState(AI.State.AIEnterBattleState));
        }
    }

    public override IEnumerator ExecuteState()
    {
        if (monsterToChangePosition != null)
        {
            if (monsterToChangePosition.GetMonsterCardData().cardPosition == CardPosition.Attack)
            {
                yield return aI.StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.ChangeToDefendPosition));
            }

            else if(monsterToChangePosition.GetMonsterCardData().cardPosition == CardPosition.DefendFacedown)
            {
                yield return aI.StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.FlipSummon));
            }

            else
            {
                yield return aI.StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.ChangeToAttackPosition));
            }

            aI.StartCoroutine(monsterToChangePosition.GetComponent<CardChangePosition>().ChangePosition(AI.Instance));

            aI.ChangeState(aI.GetAIState(AI.State.AIWaitingState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        monsterToChangePosition = null;
    }
}
