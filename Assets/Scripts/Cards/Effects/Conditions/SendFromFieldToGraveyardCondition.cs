using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFromFieldToGraveyardCondition : ConditionDefault
{
    private CardState lastStateResult;

    private CardState currentStateResult;

    private CardState lastState;

    private CardState currentState;

    public override void Initialize(Card card, Character owner)
    {
        base.Initialize(card, owner);

        this.card.OnCardStateChange += Card_OnCardStateChange;
    }

    private void Card_OnCardStateChange(object sender, Card.OnCardStateChangeEventArgs e)
    {
        lastState = e.lastState;

        currentState = e.currentState;
    }

    public override bool Condtions()
    {
        if (lastState == lastStateResult && currentState == currentStateResult)
        {
            return true;
        }

        return false;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.lastStateResult = (CardState)Enum.Parse(typeof(CardState), list[0].ToString());

        this.currentStateResult = (CardState)Enum.Parse(typeof(CardState), list[1].ToString());
    }

    public override void ResetValues()
    {
        base.ResetValues();

        lastState = default;

        currentState = default;
    }
}
