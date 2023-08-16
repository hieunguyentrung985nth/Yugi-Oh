using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpecificCardFromDeckEffect : SpellCardEffect
{
    public int numberOfCards;

    public List<Card> cardsToDraw;

    private void Awake()
    {
        cardsToDraw = new List<Card>();
    }

    public override bool ConditionsToActive()
    {
        ResetValues();

        List<Card> cardsOnDeck = owner.GetDeckZone().GetDeckCard();

        if (cardsOnDeck.Count == 0)
        {
            return false;
        }

        return true;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.cardsToDraw = list[0] as List<Card>;

        this.numberOfCards = Convert.ToInt32(values[1]);
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        if (cardsToDraw.Count != 0)
        {
            yield return owner.StartCoroutine(owner.DrawCard(numberOfCards, cardsToDraw));

            SetEffectResult(true);
        }

        yield break;
    }

    public override void ResetValues()
    {
        cardsToDraw.Clear();

        base.ResetValues();
    }
}
