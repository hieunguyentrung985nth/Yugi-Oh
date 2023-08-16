using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SanganEffect : SpellTrapDefault
{
    [SerializeField] private int conditionATK;

    [SerializeField] private int numberOfCards;

    [SerializeField] private PointToCharacter characterToShuffle;

    [SerializeField] private CardState fromState;

    [SerializeField] private CardState toState;

    private List<Card> cardsToSelect;

    private void Awake()
    {
        cardsToSelect = new List<Card>();
    }

    private void Start()
    {

    }

    public override void InitializeResolveEffect(SpellCardEffect effect = null, params object[] values)
    {
        base.InitializeResolveEffect(GetResolveCardEffects()[0], cardsToSelect, TextBoxType.AddCardFromDeckToHand);

        base.InitializeResolveEffect(GetResolveCardEffects()[1], GetResolveCardEffects()[0].GetCardSelect(), numberOfCards);

        base.InitializeResolveEffect(GetResolveCardEffects()[2], GetResolveCardEffects()[0].GetCardSelect());

        base.InitializeResolveEffect(GetResolveCardEffects()[3], TransferToCharacterType(characterToShuffle));
    }

    public override void InitializeConditionEffect(ConditionDefault effect = null, params object[] values)
    {
        base.InitializeConditionEffect(GetConditionCardEffects()[0], fromState, toState);
    }

    public override void Prepare()
    {
        ClearEffect();

        foreach (Card card in owner.GetDeckZone().GetDeckCard())
        {
            if (card is MonsterCard)
            {
                if ((card.GetCardSO() as MonsterCardSO).attackValue <= conditionATK)
                {
                    cardsToSelect.Add(card);
                }
            }         
        }
    }

    public override void ClearEffect()
    {
        cardsToSelect.Clear();
    }
}
