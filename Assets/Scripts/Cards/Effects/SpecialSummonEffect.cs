using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialSummonEffect : SpellCardEffect
{
    public enum SpecialSummonFrom
    {
        Graveyard,
        Deck,
        Hand
    }

    private SpecialSummonFrom specialSummonFrom;

    private List<Card> cardsToSpecialSummon;

    private Character character;

    private CardPosition cardPosition;

    private void Awake()
    {
        cardsToSpecialSummon = new List<Card>();
    }

    public override bool ConditionsToActive()
    {
        ResetValues();

        switch (specialSummonFrom)
        {
            case SpecialSummonFrom.Graveyard:

                if (character.GetGraveyardZone().GetMonsterCardsInGraveyard().Count > 0
                    && character.GetMonsterZone().HaveEmptyMonsterZoneSlotsOnField())
                {
                    return true;
                }

                return false;

            case SpecialSummonFrom.Deck:
                break;
            case SpecialSummonFrom.Hand:
                break;
            default:
                break;
        }

        return true;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.specialSummonFrom = (SpecialSummonFrom)Enum.Parse(typeof(SpecialSummonFrom), list[0].ToString());

        this.cardsToSpecialSummon = list[1] as List<Card>;

        this.character = (list[2] as List<Character>)[0];

        this.cardPosition = (CardPosition)Enum.Parse(typeof(CardPosition), list[3].ToString());
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        yield return StartCoroutine(HandleSpecialSummon());

        foreach (MonsterCard card in cardsToSpecialSummon.Cast<MonsterCard>())
        {
            if (cardPosition == CardPosition.Attack)
            {
                card.UpdateCardPosition(CardPosition.Attack);
            }

            else if(cardPosition == CardPosition.DefendFacedown)
            {
                card.UpdateCardPosition(CardPosition.DefendFacedown);
            }

            else
            {
                card.UpdateCardPosition(CardPosition.DefendFaceup);
            }

            character.GetGraveyardZone().RemoveCard(card);

            yield return StartCoroutine(character.SpecialSummon(card));
        }

        SetEffectResult(true);

        yield return null;
    }

    private IEnumerator HandleSpecialSummon()
    {
        switch (specialSummonFrom)
        {
            case SpecialSummonFrom.Graveyard:

                yield return StartCoroutine(cardsToSpecialSummon[0].GetOwner().GetGraveyardZone().MoveCardToTheTop(cardsToSpecialSummon[0]));

                yield return StartCoroutine(cardsToSpecialSummon[0].GetCardVisual().SelectTargetOnSelect());

                break;
            case SpecialSummonFrom.Deck:
                break;
            case SpecialSummonFrom.Hand:
                break;
            default:
                break;
        }
    }

    public override void ResetValues()
    {
        cardsToSpecialSummon.Clear();

        base.ResetValues();
    }
}
