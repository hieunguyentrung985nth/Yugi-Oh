using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCardEffect : MonoBehaviour
{
    protected Card card;

    protected CardSO cardSO;

    protected Character owner;

    protected bool confirmAction;

    protected bool effectResult;

    public void Initialize(Card card, Character owner)
    {
        this.card = card;

        this.owner = owner;

        cardSO = card.GetCardSO();
    }

    public Character GetOwner()
    {
        return owner;
    }

    public Card GetCard()
    {
        return card;
    }

    public virtual void SetUp(params object[] values)
    {

    }

    public abstract IEnumerator Resolve();

    public abstract bool ConditionsToActive();

    public bool GetEffectResult()
    {
        return effectResult;
    }

    public void SetEffectResult(bool value)
    {
        effectResult = value;
    }

    public void ConfirmAction()
    {
        confirmAction = true;
    }

    public void ResetEffect()
    {
        confirmAction = false;

        effectResult = false;
    }

    public virtual void ResetValues()
    {
        ResetEffect();
    }

    public virtual void SetCardSelect(Card card)
    {
        
    }

    public virtual List<Card> GetCardSelect()
    {
        return null;
    }

    public virtual IEnumerator RevertEffect()
    {
        yield return null;
    }
}
