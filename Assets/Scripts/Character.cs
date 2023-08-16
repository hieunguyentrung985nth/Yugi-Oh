using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public enum State
    {
        GameStateNormal,
        GameStateIdle,
        GameStateWaiting,
        GameStateBattle,
        GameStateTribute
    }

    public static event EventHandler OnCantChangePhase;

    public static event EventHandler OnCanChangePhase;

    public class OnCharacterTriggerEventEventArgs : EventArgs
    {
        public Character character;
    }

    public static event EventHandler OnHandChanged;

    [SerializeField] protected DeckZone deckZone;

    [SerializeField] protected MonsterZone monsterZone;

    [SerializeField] protected SpellTrapZone spellTrapZone;

    [SerializeField] protected int lifePoints;

    [SerializeField] protected HandZone handZone;

    [SerializeField] protected GraveyardZone graveyardZone;

    [SerializeField] private Transform attackPoint;

    public bool PlayerInputEnabled { get; private set; }

    public bool PlayerCardInteractionEnabled { get; private set; }

    public bool PlayerCardPlayOnHandEnabled { get; private set; }

    public bool PlayerChangePhaseEnabled { get; private set; }

    protected GameState currentGameState;

    protected Stack<GameState> previousGameState;

    private List<GameState> stateList;

    protected bool havePlayedMonsterThatTurn;

    private float moveTimer = 0f;

    private float moveTimerMax = 0.5f;

    protected virtual void Awake()
    {
        stateList = new List<GameState>();

        previousGameState = new Stack<GameState>();
    }

    protected virtual void Start()
    {
        SetOwner();

        deckZone.Initialize();

        InitializeStates();

        previousGameState.Push(GetGameState(State.GameStateNormal));

        ChooseTurnUI.Instance.OnChooseTurn += ChooseTurnUI_OnChooseTurn;

        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        BattleSystem.Instance.OnActionsFinished += BattleSystem_OnActionsFinished;
    }

    private void BattleSystem_OnActionsFinished(object sender, OnCharacterTriggerEventEventArgs e)
    {
        if (previousGameState.Count == 1)
        {
            ChangeState(previousGameState.Peek());
        }

        else
        {
            previousGameState.Pop();

            ChangeState(previousGameState.Peek());
        }
    }

    private void TurnManager_OnChangeTurn(object sender, EventArgs e)
    {
        previousGameState.Clear();

        if (TurnManager.Instance.GetCurrentTurn() == this)
        {
            ChangeState(GetGameState(State.GameStateNormal));
        }

        else
        {
            ChangeState(GetGameState(State.GameStateIdle));
        }
    }

    private void ChooseTurnUI_OnChooseTurn(object sender, ChooseTurnUI.OnChooseTurnEventArgs e)
    {
        if (e.isPlayerGoFirst)
        {
            Player.Instance.ChangeState(GetGameState(State.GameStateNormal));
        }

        else
        {
            AI.Instance.ChangeState(GetGameState(State.GameStateNormal));
        }
    }

    private void ChangeToSelectState()
    {
        TurnOffPlayerInput();

        TurnOffPlayerCardPlayOnHand();

        TurnOffPlayerChangePhase();
    }

    private void ChangeToNormalState()
    {
        TurnOnPlayerInput();

        TurnOnPlayerCardPlayOnHand();

        TurnOnPlayerChangePhase();
    }

    private void InitializeStates()
    {
        stateList.Add(new GameStateNormal(this));

        stateList.Add(new GameStateIdle(this));

        stateList.Add(new GameStateBattle(this));

        stateList.Add(new GameStateWaiting(this));

        stateList.Add(new GameStateTribute(this));
    }

    public void ChangeState(GameState newState)
    {
        if (currentGameState != null)
        {
            if (newState != GetGameState(State.GameStateWaiting) 
                && !previousGameState.Contains(newState))
            {
                previousGameState.Push(newState);
            }

            currentGameState.ExitState();
        }

        currentGameState = newState;

        currentGameState.EnterState();
    }

    public void UndoState()
    {
        GameState preState = previousGameState.Pop();

        if (UndoAction.Instance.GetUndoActionState() == UndoAction.UndoActionState.Tribute)
        {
            ChangeState(GetGameState(State.GameStateWaiting));
        }

        else
        {
            ChangeState(preState);
        }
    }

    public GameState GetGameState(State state)
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            if (stateList[i].GetType().Name == state.ToString())
            {
                return stateList[i];
            }
        }

        return null;
    }

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public void TurnOnPlayerInput()
    {
        PlayerInputEnabled = true;
    }

    public void TurnOffPlayerInput()
    {
        PlayerInputEnabled = false;
    }

    public void TurnOnPlayerCardInteraction()
    {
        PlayerCardInteractionEnabled = true;
    }

    public void TurnOffPlayerCardInteraction()
    {
        PlayerCardInteractionEnabled = false;
    }

    public void TurnOnPlayerCardPlayOnHand()
    {
        PlayerCardPlayOnHandEnabled = true;
    }

    public void TurnOffPlayerChangePhase()
    {
        PlayerChangePhaseEnabled = false;

        OnCantChangePhase?.Invoke(this, EventArgs.Empty);
    }

    public void TurnOnPlayerChangePhase()
    {
        PlayerChangePhaseEnabled = true;

        OnCanChangePhase?.Invoke(this, EventArgs.Empty);
    }

    public void TurnOffPlayerCardPlayOnHand()
    {
        PlayerCardPlayOnHandEnabled = false;
    }

    private void SetOwner()
    {
        deckZone.SetOwner(this);

        graveyardZone.SetOwner(this);
    }

    public GraveyardZone GetGraveyardZone()
    {
        return graveyardZone;
    }

    public abstract IEnumerator DrawCard(int numberOfCards = 1, List<Card> cardsToDraw = null);

    public void PlayCardFromHand(Card card)
    {
        BattleSystem.Instance.SetCharacterAction(this);

        CardInfoUI.Instance.ExitHovering();

        if (card is MonsterCard)
        {
            MonsterZoneSingle zoneSlot = monsterZone.GetEmptyMonsterSlot();

            StartCoroutine(PlayMonsterCardFromHand(card as MonsterCard, zoneSlot));
        }

        else if (card is SpellCard)
        {
            SpellTrapZoneSingle zoneSlot = spellTrapZone.GetEmptySpellTrapSlot();

            StartCoroutine(PlaySpellCardFromHand(card as SpellCard, zoneSlot));
        }

        ChangeState(GetGameState(State.GameStateWaiting));
    }

    protected virtual void SummonMonsterCardOnAttackPosition(MonsterCard monsterCard)
    {
        monsterCard.GetCardVisual().AttackPosition();
    }

    protected virtual void SummonMonsterCardOnDefendPosition(MonsterCard monsterCard)
    {
        monsterCard.GetCardVisual().SetDefendFaceDownPosition();
    }

    protected virtual IEnumerator DefaultSummon(MonsterCard monsterCard, MonsterZoneSingle zoneSlot)
    {
        monsterCard.UpdateCardState(CardState.Field);

        monsterCard.TurnOffCanChangePosition();

        zoneSlot.SetMonsterForThisSlot(monsterCard);

        monsterCard.SetMonsterZoneSingle(zoneSlot);

        TributeManager.Instance.AddMonsterToTributeList(monsterCard);

        CardInfoUI.Instance.ShowCardImmediately(monsterCard);

        yield return null;
    }

    public virtual IEnumerator PlayMonsterCardFromHand(MonsterCard monsterCard, MonsterZoneSingle zoneSlot)
    {
        havePlayedMonsterThatTurn = true;

        monsterCard.GetComponent<CardChangePosition>().SetHavePlayedThatTurn();

        yield return StartCoroutine(DefaultSummon(monsterCard, zoneSlot));

        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.Attack)
        {
            SummonMonsterCardOnAttackPosition(monsterCard);
        }

        else
        {
            SummonMonsterCardOnDefendPosition(monsterCard);
        }

        handZone.RemoveCard(monsterCard);

        TributeManager.Instance.AddMonsterToTributeList(monsterCard);

        yield return StartCoroutine(MoveCardToPosition(monsterCard, zoneSlot.transform));
    }

    public virtual IEnumerator SpecialSummon(MonsterCard monsterCard)
    {
        MonsterZoneSingle zoneSlot = monsterZone.GetEmptyMonsterSlot();

        yield return StartCoroutine(DefaultSummon(monsterCard, zoneSlot));

        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.Attack)
        {
            monsterCard.GetCardVisual().AttackPosition();
        }

        else if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.DefendFacedown)
        {
            monsterCard.GetCardVisual().DefendFaceDownPosition();
        }

        else
        {
            monsterCard.GetCardVisual().DefendFaceUpPosition();
        }

        yield return StartCoroutine(SetCardToPosition(monsterCard, zoneSlot.transform));

        yield return StartCoroutine(monsterCard.GetCardVisual().SpecialSummon());
    }

    public virtual IEnumerator PlaySpellCardFromHand(SpellCard spellCard, SpellTrapZoneSingle zoneSlot)
    {
        zoneSlot.SetSpellTrapForThisSlot(spellCard);

        spellCard.SetSpellTrapZoneSingle(zoneSlot);

        if (spellCard.GetCardStatus() == CardStatus.CanPlay || spellCard.GetSpellCardData().spellCardState == SpellCardState.Facedown)
        {
            SetSpellCard(spellCard);
        }

        else
        {
            yield return StartCoroutine(PlaySpellCardActive(spellCard));
        }

        handZone.RemoveCard(spellCard);

        yield return StartCoroutine(MoveCardToPosition(spellCard, zoneSlot.transform));
    }

    protected virtual void SetSpellCard(SpellCard spellCard)
    {
        spellCard.UpdateCardState(CardState.Field);

        spellCard.GetCardVisual().FaceDownOnField();

        EffectsManager.Instance.AddSpellSpeed1FacedownEffect(spellCard.GetSpellTrapDefault());     
    }

    protected virtual IEnumerator PlaySpellCardActive(SpellCard spellCard)
    {
        spellCard.UpdateCardState(CardState.Field);

        spellCard.GetCardVisual().FaceUpOnField();

        //EffectsManager.Instance.AddNormalSpellEffect(spellCard.GetSpellTrapDefault());

        EffectsManager.Instance.AddStackEffect(spellCard.GetSpellTrapDefault());

        yield return null;
    }

    public virtual IEnumerator ActiveSpellCardOnField(SpellCard spellCard)
    {
        spellCard.GetCardVisual().CardNormalStateOnField();

        EffectsManager.Instance.AddStackEffect(spellCard.GetSpellTrapDefault());

        yield return StartCoroutine(spellCard.GetSpellTrapDefault().GetSpellTrapCard().GetCardVisual().FlipSpellTrap());
    }

    private IEnumerator MoveCardToPosition(Card card, Transform targetTransform)
    {
        Vector3 startPos = card.transform.position;

        while (moveTimer < moveTimerMax)
        {
            moveTimer += Time.deltaTime;

            float t = Mathf.Clamp01(moveTimer / moveTimerMax);

            card.transform.position = Vector3.Lerp(startPos, targetTransform.position, t);

            yield return null;
        }

        card.transform.position = targetTransform.position;

        moveTimer = 0;

        card.transform.SetParent(targetTransform);

        card.transform.localPosition = Vector3.zero;

        OnHandChanged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SetCardToPosition(Card card, Transform targetTransform)
    {
        card.transform.position = targetTransform.position;

        card.transform.SetParent(targetTransform);

        card.transform.localPosition = Vector3.zero;

        //yield return StartCoroutine(EffectsManager.Instance.ExecuteTriggerMonsterEffects());

        //yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());

        yield return null;
    }


    public bool HavePlayedMonsterThatTurn()
    {
        return havePlayedMonsterThatTurn;
    }

    public HandZone GetHandZone()
    {
        return handZone;
    }

    public abstract bool CanAction();

    public MonsterZone GetMonsterZone()
    {
        return monsterZone;
    }

    public SpellTrapZone GetSpellTrapZone()
    {
        return spellTrapZone;
    }

    public Transform GetAttackPoint()
    {
        return attackPoint;
    }

    public DeckZone GetDeckZone()
    {
        return deckZone;
    }

    public int GetLifePoints()
    {
        return lifePoints;
    }

    public void DecreaseLifePoints(int value)
    {
        lifePoints = Mathf.Clamp(lifePoints - value, 0, lifePoints);
    }

    public void IncreaseLifePoints(int value)
    {
        lifePoints += value;
    }

    public void CheckCardCanPlay()
    {
        foreach (Card card in GetHandZone().GetHandCard())
        {
            GameRules.Instance.CheckCardCanPlay(card, this);
        }
    }

    public virtual IEnumerator RevealCardOnHand(Card card)
    {
        yield return StartCoroutine(card.GetCardVisual().RevealCardOnHand());
    }
}
