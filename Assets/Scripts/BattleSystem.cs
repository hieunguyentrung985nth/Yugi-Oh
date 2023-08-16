using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }

    public enum ActionType
    {
        None,
        Tribute,
        SelectUI
    }

    public event EventHandler<Character.OnCharacterTriggerEventEventArgs> OnActionsFinished;

    public event EventHandler<OnCarryActionEventArgs> OnCarryAction;

    public class OnCarryActionEventArgs : EventArgs
    {
        public ActionType type;
    }

    public event EventHandler<OnCardHoverTooltipEventArgs> OnCardOnHandHoverTooltip;

    public event EventHandler<OnCardHoverTooltipEventArgs> OnCardChangePositionHoverTooltip;

    public event EventHandler<OnCardHoverTooltipEventArgs> OnCardAttackHoverTooltip;

    public event EventHandler<OnCardHoverTooltipEventArgs> OnCardActiveHoverTooltip;

    public class OnCardHoverTooltipEventArgs : EventArgs
    {
        public Card card;
    }

    public event EventHandler OnCheckGameOver;

    public bool firstTurn;

    private ActionType type;

    private Character character;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //TurnManager.Instance.OnStartGame += TurnManager_OnStartGame;

        TurnManager.Instance.OnStartGame += TurnManager_OnStartGame;

        PhaseManager.Instance.OnEndPhase += PhaseManager_OnEndPhase;

        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;
    }

    private IEnumerator TurnManager_OnStartGame()
    {
        firstTurn = true;

        yield return null;
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        //TurnOffChangePositionCardsOnField();

        //TurnOnChangePositionForCardsOnField();
    }

    private void TurnManager_OnChangeTurn(object sender, EventArgs e)
    {
        
    }

    private void PhaseManager_OnEndPhase(object sender, EventArgs e)
    {
        firstTurn = false;

        //TurnOffChangePositionForCurrentTurnCardsOnField();
    }

    private void TurnManager_OnStartGame(object sender, EventArgs e)
    {
        firstTurn = true;
    }

    public void SetCharacterAction(Character character)
    {
        this.character = character;
    }

    public void OnActionsFinishedEvent()
    {
        OnActionsFinished?.Invoke(this, new Character.OnCharacterTriggerEventEventArgs
        {
            character = this.character
        });

        this.character = null;
    }

    public void SetActionType(ActionType type)
    {
        this.type = type;
    }

    public ActionType GetActionType()
    {
        return this.type;
    }

    public void ResetActionType()
    {
        this.type = default;
    }

    public void OnCarryActionEvent()
    {
        OnCarryAction?.Invoke(this, new OnCarryActionEventArgs
        {
            type = type
        });

        ResetActionType();
    }

    public void TooltipCardOnHandEvent(Card card)
    {
        OnCardOnHandHoverTooltip?.Invoke(this, new OnCardHoverTooltipEventArgs
        {
            card = card
        });
    }

    public void TooltipCardChangePositionEvent(Card card)
    {
        OnCardChangePositionHoverTooltip?.Invoke(this, new OnCardHoverTooltipEventArgs
        {
            card = card
        });
    }

    public void TooltipCardAttackEvent(Card card)
    {
        OnCardAttackHoverTooltip?.Invoke(this, new OnCardHoverTooltipEventArgs
        {
            card = card
        });
    }

    public void TooltipCardActiveEvent(Card card)
    {
        OnCardActiveHoverTooltip?.Invoke(this, new OnCardHoverTooltipEventArgs
        {
            card = card
        });
    }

    public void CheckGameOverEvent()
    {
        OnCheckGameOver?.Invoke(this, EventArgs.Empty);
    }
}
