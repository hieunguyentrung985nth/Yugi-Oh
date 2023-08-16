using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllSpellAndTrapCardsOnFieldEffect : SpellCardEffect
{
    public override void SetUp(params object[] values)
    {
        
    }

    public override bool ConditionsToActive()
    {
        return true;
    }

    public override IEnumerator Resolve()
    {
        List<SpellTrapCard> list = new List<SpellTrapCard>();

        list.AddRange(TurnManager.Instance.GetNotCurrentTurn().GetSpellTrapZone().GetSpellTrapCardsOnField());

        list.AddRange(TurnManager.Instance.GetCurrentTurn().GetSpellTrapZone().GetSpellTrapCardsOnField());

        foreach (SpellTrapCard card in list)
        {
            if (card != this.card)
            {
                yield return StartCoroutine(card.SetCardToGraveyard());
            }
        }
    }
}
