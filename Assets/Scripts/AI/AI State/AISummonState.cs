using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISummonState : AIState
{
    private MonsterCard monsterToSummon;

    public AISummonState(AI aI) : base(aI)
    {
    }

    public override void EnterState()
    {
        List<MonsterCard> monsterCardsOnHand = AI.Instance.GetHandZone().GetMonsterCardsOnHand();

        List<MonsterCard> monsterCardsOnField = AI.Instance.GetMonsterZone().GetMonsterCardsOnField();

        bool done = false;

        foreach (MonsterCard monsterCard in monsterCardsOnHand)
        {
            if (monsterCardsOnField.Count == 0)
            {
                if (GameRules.Instance.CheckCardCanPlay(monsterCard, AI.Instance))
                {                   
                    monsterToSummon = monsterCard;

                    break;
                }
            }

            foreach (MonsterCard monsterOnField in monsterCardsOnField)
            {
                if (GameRules.Instance.CheckCardCanPlay(monsterCard, AI.Instance))
                {
                    if (monsterCard.NumberForTribute() > 0)
                    {
                        if (monsterCard.GetMonsterCardData().attackValue < monsterOnField.GetMonsterCardData().attackValue)
                        {
                            continue;
                        }
                    }

                    monsterToSummon = monsterCard;

                    done = true;

                    break;
                }
            }

            if (done) break;
        }

        if (monsterToSummon != null)
        {
            base.EnterState();
        }

        else
        {
            aI.ChangeState(aI.GetAIState(AI.State.AIChangePosition));
        }


        //if (GameRules.Instance.CheckCardCanPlay(monsterCard, AI.Instance))
        //{
        //    monsterToSummon = monsterCard;

            
        //}

        //else
        //{
            
        //}
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();

        monsterToSummon = null;
    }

    public override IEnumerator ExecuteState()
    {
        Debug.Log("Summon!");

        //aI.ChangeState(aI.GetAIState(AI.State.AIEndTurnState));

        if (monsterToSummon != null)
        {
            //monsterToSummon.UpdateCardPosition(CardPosition.DefendFacedown);

            yield return aI.StartCoroutine(AIReact.Instance.AIReactionForSpecialCards(monsterToSummon, AIReaction.SummonMonsterWithTribute));

            CardPlayHandManager.Instance.AIPlayMonsterCard(monsterToSummon);

            //AI.Instance.PlayCardFromHand(monsterToSummon);

            aI.ChangeState(aI.GetAIState(AI.State.AIWaitingState));
        }
    }
}
