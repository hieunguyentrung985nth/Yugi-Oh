using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHasCardsCondition : ConditionDefault
{
    public override void Initialize(Card card, Character owner)
    {
        base.Initialize(card, owner);
    }

    public override bool Condtions()
    {
        return owner.GetDeckZone().GetDeckCard().Count > 0;
    }

    public override void SetUp(params object[] values)
    {

    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
