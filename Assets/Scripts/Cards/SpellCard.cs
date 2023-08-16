using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : SpellTrapCard
{
    [SerializeField] private SpellTrapDefault cardEffectObject;

    private Transform effectTransform;

    private SpellTrapDefault spellTrapDefault;

    private List<SpellCardEffect> resolveCardEffects;

    private List<SpellCardEffect> activationCardEffects;

    private List<ConditionDefault> conditionCardEffects;

    private SpellCardData spellCardData;

    protected override void Start()
    {
        base.Start();

        effectTransform = GameManager.Instance.GetEffectTransform();

        InitializeEffect();

        InitializeCard();
    }

    private void InitializeEffect()
    {
        spellTrapDefault = Instantiate(cardEffectObject, effectTransform);

        spellTrapDefault.Initialized();

        spellTrapDefault.SetUp(this, owner);

        spellTrapDefault.InitializeResolveEffect();

        spellTrapDefault.InitializeActivationEffect();

        spellTrapDefault.InitializeConditionEffect();

        resolveCardEffects = spellTrapDefault.GetResolveCardEffects();

        activationCardEffects = spellTrapDefault.GetActivationCardEffects();

        conditionCardEffects = spellTrapDefault.GetConditionCardEffects();

        foreach (SpellCardEffect effect in resolveCardEffects)
        {
            effect.Initialize(this, owner);
        }

        foreach (SpellCardEffect effect in activationCardEffects)
        {
            effect.Initialize(this, owner);
        }

        foreach (ConditionDefault condition in conditionCardEffects)
        {
            condition.Initialize(this, owner);
        }
    }

    public void InitializeCard()
    {
        SpellCardSO spellCardSO = cardSO as SpellCardSO;

        spellCardData = new SpellCardData();
    }

    public SpellCardData GetSpellCardData()
    {
        return spellCardData;
    }

    public void UpdateSpellCardState(SpellCardState state)
    {
        spellCardData.spellCardState = state;
    }

    public bool CanActiveCard()
    {
        spellTrapDefault.Prepare();

        return spellTrapDefault.ConditionsForSpell();
    }

    public override bool CanPlayCard()
    {
        return true;   
    }

    public override void DefaultStateOnField()
    {
        UpdateCardState(SpellCardState.Faceup);

        UpdateCardStatus(CardStatus.CantActive);
    }

    public override IEnumerator SetCardToGraveyard()
    {
        GetSpellTrapZoneSingle().HideText();

        GetSpellTrapZoneSingle().ResetSlot();

        yield return StartCoroutine(base.SetCardToGraveyard());

        UpdateCardState(CardState.Graveyard);

        GetCardVisual().DefaultCardOnField();

        DefaultStateOnField();
    }

    public override IEnumerator SendCardToGraveyard()
    {
        GetSpellTrapZoneSingle().HideText();

        GetSpellTrapZoneSingle().ResetSlot();

        yield return StartCoroutine(base.SendCardToGraveyard());

        DefaultStateOnField();

        GetCardVisual().DefaultCardOnField();
    }

    public override IEnumerator DestroyCard()
    {
        yield return StartCoroutine(base.DestroyCard());

        //yield return StartCoroutine(spellTrapDefault.BehaveAfterDestroyed());

        foreach (SpellCardEffect effect in spellTrapDefault.GetResolveCardEffects())
        {
            yield return StartCoroutine(effect.RevertEffect());
        }

        foreach (SpellCardEffect effect in spellTrapDefault.GetResolveCardEffects())
        {
            effect.ResetValues();
        }

        foreach (SpellCardEffect effect in spellTrapDefault.GetActivationCardEffects())
        {
            effect.ResetValues();
        }

        EffectsManager.Instance.RemoveSpellSpeed1FacedownEffect(spellTrapDefault);
    }

    public void UpdateCardState(SpellCardState state)
    {
        spellCardData.spellCardState = state;
    }

    public List<SpellCardEffect> GetSpellCardEffect()
    {
        return resolveCardEffects;
    }

    public override void Prepare()
    {
        spellTrapDefault.Prepare();
    }

    public SpellTrapDefault GetSpellTrapDefault()
    {
        return spellTrapDefault;
    }

    public override bool IsFaceUp()
    {
        return spellCardData.spellCardState == SpellCardState.Faceup && cardData.cardState != CardState.Hand;
    }
}
