using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    public static float offsetZCard { get => -0.01f;}

    public event EventHandler<OnCardStateChangeEventArgs> OnCardStateChange;

    public class OnCardStateChangeEventArgs : EventArgs
    {
        public CardState lastState;

        public CardState currentState;
    }

    [SerializeField] private CardVisual cardVisual;

    [SerializeField] protected CardSO cardSO;

    protected CardData cardData;

    private HorizontalLayoutGroup horizontalLayoutGroup;

    protected Character owner;

    private float timeDelayBeforeMoving = 0.25f;

    private float timeDelayAfterMoving = 0.25f;

    private float timeSendToGraveyard = 0.25f;

    private void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    public void SetUp()
    {
        cardVisual.SetUpCardVisual(cardSO);

        cardData = new CardData();
    }

    public CardSO GetCardSO()
    {
        return cardSO;
    }

    public CardVisual GetCardVisual()
    {
        return cardVisual;
    }

    public CardState GetCardState()
    {
        return cardData.cardState;
    }

    public void SetHorizontalLayoutGroup(HorizontalLayoutGroup horizontalLayoutGroup)
    {
        this.horizontalLayoutGroup = horizontalLayoutGroup;
    }

    public HorizontalLayoutGroup GetHorizontalLayoutGroup()
    {
        return horizontalLayoutGroup;
    }

    public void UpdateCardState(CardState cardState)
    {
        OnCardStateChange?.Invoke(this, new OnCardStateChangeEventArgs
        {
            lastState = cardData.cardState,

            currentState = cardState
        });

        cardData.cardState = cardState;
    }

    public void UpdateCardStatus(CardStatus cardStatus)
    {
        cardData.cardStatus = cardStatus;
    }

    public CardStatus GetCardStatus()
    {
        return cardData.cardStatus;
    }

    public abstract bool CanPlayCard();

    public abstract void DefaultStateOnField();

    public bool IsCardOnHand()
    {
        Character owner = TurnManager.Instance.GetCurrentTurn();

        List<Card> cardsOnHandList = owner.GetHandZone().GetHandCard();

        if (cardsOnHandList.Contains(this) && cardData.cardState == CardState.Hand)
        {
            return true;
        }

        return false;
    }

    public bool IsCardOnField()
    {
        return cardData.cardState == CardState.Field;
    }

    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }

    public void ResetState()
    {
        UpdateCardState(CardState.Graveyard);

        cardVisual.DefaultCardOnField();

        cardVisual.FaceUpOnField();
    }

    public virtual IEnumerator Tribute()
    {
        yield return new WaitForSeconds(timeDelayAfterMoving);

        LeanTween.move(gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, owner.GetGraveyardZone().GetGraveyardCards().Count * offsetZCard), 0f)
            .setFrom(transform.position)
            .setOnComplete(() =>
            {
                owner.GetGraveyardZone().AddCard(this);

                ResetState();
            });
    }

    public virtual IEnumerator DestroyCard()
    {
        ResetState();

        yield return new WaitForSeconds(timeDelayAfterMoving);

        // ...
    }

    public virtual IEnumerator SendCardToGraveyard()
    {
        yield return new WaitForSeconds(timeDelayBeforeMoving);

        LeanTween.move(gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, owner.GetGraveyardZone().GetGraveyardCards().Count * offsetZCard), timeSendToGraveyard)
            .setFrom(transform.position)
            .setOnComplete(() =>
            {
                owner.GetGraveyardZone().AddCard(this);
            });

        yield return StartCoroutine(DestroyCard());
    }

    public virtual IEnumerator SetCardToGraveyard()
    {        
        yield return new WaitForSeconds(timeDelayBeforeMoving);

        LeanTween.move(gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, owner.GetGraveyardZone().GetGraveyardCards().Count * offsetZCard), 0f)
            .setFrom(transform.position)
            .setOnComplete(() =>
            {
                owner.GetGraveyardZone().AddCard(this);
            });

        yield return StartCoroutine(DestroyCard());
    }

    public Character GetOwner()
    {
        return owner;
    }

    public abstract void Prepare();

    public abstract bool IsFaceUp();

    public bool IsCardOnDeck()
    {
        return cardData.cardState == CardState.Deck;
    }
}
