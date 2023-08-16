using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCardInHandEffect : SpellCardEffect
{
    public List<Card> cardsToReveal;

    private void Awake()
    {
        cardsToReveal = new List<Card>();
    }

    public override bool ConditionsToActive()
    {       
        return true;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.cardsToReveal = list[0] as List<Card>;
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        if (cardsToReveal.Count != 0)
        {
            foreach (Card card in cardsToReveal)
            {
                yield return StartCoroutine(owner.RevealCardOnHand(card));
            }

            SetEffectResult(true);
        }

        yield break;
    }

    public override void ResetValues()
    {
        cardsToReveal.Clear();

        base.ResetValues();
    }
}
