using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class HaveAnySpellTrapOnFieldCondition : ConditionDefault
{
    public override void Initialize(Card card, Character owner)
    {
        base.Initialize(card, owner);
    }

    public override void SetUp(params object[] values)
    {

    }

    public override bool Condtions()
    {
        List<SpellTrapCard> aICards = AI.Instance.GetSpellTrapZone().GetSpellTrapCardsOnField();

        List<SpellTrapCard> playerCards = Player.Instance.GetSpellTrapZone().GetSpellTrapCardsOnField();

        if (aICards.Count > 0 || playerCards.Count > 0)
        {
            if (owner.GetSpellTrapZone().GetSpellTrapCardsOnField().Count == 1)
            {
                if (owner.GetSpellTrapZone().GetSpellTrapCardsOnField()[0] == this.card)
                {
                    return false;
                }
            }
            
            return true;
        }

        return false;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
