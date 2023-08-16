using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveEnoughLifePointsCondition : ConditionDefault
{
    private int pointNeeded;

    public override void Initialize(Card card, Character owner)
    {
        base.Initialize(card, owner);
    }

    public override void SetUp(params object[] values)
    {
        this.pointNeeded = (int)values[0];
    }

    public override bool Condtions()
    {
        return owner.GetLifePoints() > pointNeeded;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
