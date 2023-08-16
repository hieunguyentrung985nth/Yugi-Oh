using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveMonstersInMyGraveyardCondition : ConditionDefault
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
        return owner.GetGraveyardZone().GetMonsterCardsInGraveyard().Count > 0;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
