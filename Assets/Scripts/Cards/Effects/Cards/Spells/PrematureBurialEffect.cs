using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrematureBurialEffect : SpellTrapDefault
{
    [SerializeField] private int pointToPay;

    [SerializeField] private PointToZone specialSummonFrom;

    [SerializeField] private PointToCharacter character;

    [SerializeField] private CardPosition cardPosition;

    private List<Card> cardsToSelect;

    private Card cardToRespawn;

    private void Awake()
    {
        cardsToSelect = new List<Card>();
    }

    private void Start()
    {

    }

    public override void InitializeResolveEffect(SpellCardEffect effect = null, params object[] values)
    {
        base.InitializeResolveEffect(GetResolveCardEffects()[0], cardsToSelect, TextBoxType.SpecialSummonFromGraveyard);

        base.InitializeResolveEffect(GetResolveCardEffects()[1], specialSummonFrom, GetResolveCardEffects()[0].GetCardSelect(), TransferToCharacterType(character), cardPosition);

        base.InitializeResolveEffect(GetResolveCardEffects()[2], GetResolveCardEffects()[0].GetCardSelect());
    }

    public override void InitializeActivationEffect(SpellCardEffect effect = null, params object[] values)
    {
        base.InitializeActivationEffect(GetActivationCardEffects()[0], pointToPay);
    }

    public override void InitializeConditionEffect(ConditionDefault effect = null, params object[] values)
    {
        base.InitializeConditionEffect(GetConditionCardEffects()[0], pointToPay);
    }

    public override void Prepare()
    {
        ClearEffect();

        List<Card> graveyardCards = owner.GetGraveyardZone().GetGraveyardCards();

        foreach (Card card in graveyardCards)
        {
            if (card is MonsterCard)
            {
                cardsToSelect.Add(card);
            }
        }
    }

    public override void ClearEffect()
    {
        cardsToSelect.Clear();
    }
}
