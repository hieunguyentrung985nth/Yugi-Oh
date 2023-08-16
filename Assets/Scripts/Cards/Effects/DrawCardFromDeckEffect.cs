using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardFromDeckEffect : SpellCardEffect
{
    public int numberOfCards;

    public List<Card> cardsToDraw;

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
        this.numberOfCards = Convert.ToInt32(values[0]);
    }

    public override IEnumerator Resolve()
    {
        List<Card> cardsOnDeck = owner.GetDeckZone().GetDeckCard();

        if (cardsOnDeck.Count >= numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                cardsToDraw.Add(cardsOnDeck[i]);
            }
        }

        else if (cardsOnDeck.Count < numberOfCards && cardsOnDeck.Count > 0)
        {
            for (int i = 0; i < numberOfCards - cardsOnDeck.Count; i++)
            {
                cardsToDraw.Add(cardsOnDeck[i]);
            }
        }

        SetEffectResult(false);

        if (cardsToDraw.Count > 0)
        {
            yield return owner.StartCoroutine(owner.DrawCard(numberOfCards, cardsToDraw));

            SetEffectResult(true);
        }
    }

    public override void ResetValues()
    {
        cardsToDraw.Clear();

        base.ResetValues();
    }
}
