using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Character
{
    public static AI Instance { get; private set; }

    public event EventHandler<OnAIPlayMonsterCardFromHandEventArgs> OnAIPlayMonsterCardFromHand;

    public class OnAIPlayMonsterCardFromHandEventArgs : EventArgs
    {
        public Card card;
    }

    public new enum State
    {
        AIIdleState,
        AIWaitingState,
        AIDrawState,
        AISummonState,
        AIChangePosition,
        AIEnterBattleState,
        AIAttackState,
        AIEndTurnState
    }

    [SerializeField] private AIReact aiReact;

    private List<AIState> aIStateList;

    private AIState currentState;

    protected new void Awake()
    {
        base.Awake();

        Instance = this;

        aIStateList = new List<AIState>();
    }

    protected override void Start()
    {
        base.Start();

        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        InitializeStates();

        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;

        MonsterCard.OnMonsterCardDestroyed += MonsterCard_OnMonsterCardDestroyed;

        ChangeState(GetAIState(State.AIIdleState));
    }

    private void MonsterCard_OnMonsterCardDestroyed(object sender, MonsterCard.OnMonsterCardDestroyedEventArgs e)
    {
        if (e.monsterCard.GetOwner() == this && e.monsterCard.NumberForTribute() != 0)
        {
            StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.MonsterGotDestroyed));
        }
    }

    private void OnDestroy()
    {
        MonsterCard.OnMonsterCardDestroyed -= MonsterCard_OnMonsterCardDestroyed;
    }

    private void TurnManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        havePlayedMonsterThatTurn = false;
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        if (TurnManager.Instance.IsAITurn())
        {
            HandlePhase(e.phase);
        }
    }

    protected void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    private void InitializeStates()
    {
        aIStateList.Add(new AIIdleState(this));

        aIStateList.Add(new AIDrawState(this));

        aIStateList.Add(new AIWaitingState(this));

        aIStateList.Add(new AISummonState(this));

        aIStateList.Add(new AIChangePosition(this));

        aIStateList.Add(new AIEnterBattleState(this));

        aIStateList.Add(new AIAttackState(this));

        aIStateList.Add(new AIEndTurnState(this));
    }

    public void ChangeState(AIState newState)
    {
        base.ChangeState(GetGameState(Character.State.GameStateNormal));

        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;

        currentState.EnterState();
    }

    public AIState GetAIState(State state)
    {
        for (int i = 0; i < aIStateList.Count; i++)
        {
            if (aIStateList[i].GetType().Name == state.ToString())
            {
                return aIStateList[i];
            }
        }

        return null;
    }

    public AIState GetCurrentAIState()
    {
        return currentState;
    }

    public void HandlePhase(PhaseManager.Phase phase)
    {
        switch (phase)
        {
            case PhaseManager.Phase.Draw:

                ChangeState(GetAIState(State.AIDrawState));

                break;
            case PhaseManager.Phase.Standby:

                ChangeState(GetAIState(State.AIIdleState));

                break;
            case PhaseManager.Phase.Main1:

                ChangeState(GetAIState(State.AISummonState));

                break;
            case PhaseManager.Phase.Battle:
                break;
            case PhaseManager.Phase.Main2:
                break;
            case PhaseManager.Phase.End:

                ChangeState(GetAIState(State.AIIdleState));

                break;
            default:
                break;
        }
    }

    public override IEnumerator DrawCard(int numberOfCards = 1, List<Card> cardsToDraw = null)
    {
        int currentIndex = 0;

        List<Card> cardList = deckZone.GetDeckCard();

        if (cardsToDraw != null)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                foreach (Card card in cardList)
                {
                    if (card == cardsToDraw[i])
                    {
                        deckZone.RemoveCard(card);

                        card.GetCardVisual().FaceDownOnHand();

                        yield return StartCoroutine(handZone.AddCard(card));

                        yield return null;

                        break;
                    }
                }
            }
        }

        else
        {
            while (cardList.Count > 0 && currentIndex < numberOfCards)
            {
                Card card = cardList[cardList.Count - 1];

                deckZone.RemoveCard(card);

                currentIndex++;

                card.GetCardVisual().FaceDownOnHand();

                yield return StartCoroutine(handZone.AddCard(card));

                yield return null;
            }
        }
    }

    public override bool CanAction()
    {
        return true;
    }

    public override IEnumerator PlayMonsterCardFromHand(MonsterCard monsterCard, MonsterZoneSingle zoneSlot)
    {
        yield return StartCoroutine(base.PlayMonsterCardFromHand(monsterCard, zoneSlot));

        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.DefendFacedown)
        {
            monsterCard.GetMonsterZoneSingle().HideText();
        }

        else
        {
            monsterCard.GetMonsterZoneSingle().SetUpText();
        }

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());

        yield return StartCoroutine(GetCurrentAIState().AIDelay());
    }

    protected override IEnumerator DefaultSummon(MonsterCard monsterCard, MonsterZoneSingle zoneSlot)
    {
        OnAIPlayMonsterCardFromHand?.Invoke(this, new OnAIPlayMonsterCardFromHandEventArgs
        {
            card = monsterCard
        });


        yield return StartCoroutine(base.DefaultSummon(monsterCard, zoneSlot));
    }

    public override IEnumerator RevealCardOnHand(Card card)
    {
        yield return StartCoroutine(base.RevealCardOnHand(card));

        card.GetCardVisual().FaceDownOnHand();
    }

    public override IEnumerator PlaySpellCardFromHand(SpellCard spellCard, SpellTrapZoneSingle zoneSlot)
    {
        yield return StartCoroutine(base.PlaySpellCardFromHand(spellCard, zoneSlot));

        if (spellCard.GetSpellCardData().spellCardState == SpellCardState.Faceup)
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.PlaySpellCard));
        }

        else
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.SetSpellTrapCard));
        }

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());
    }

    public override IEnumerator ActiveSpellCardOnField(SpellCard spellCard)
    {
        yield return StartCoroutine(base.ActiveSpellCardOnField(spellCard));

        yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.PlaySpellCard));

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());
    }
}
