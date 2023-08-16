using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerSpecialMonster;

    private const float TIME_DELAY_BETWEEN_ACTIONS = 1f;

    private bool canAction;

    protected new void Awake()
    {
        base.Awake();

        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        BoxUI.Instacne.OnBoxShow += BoxUI_OnBoxShow;

        BoxUI.Instacne.OnBoxCancel += BoxUI_OnBoxCancel;

        BattleSystem.Instance.OnCarryAction += BattleSystem_OnCarryAction;
    }

    private void BattleSystem_OnCarryAction(object sender, BattleSystem.OnCarryActionEventArgs e)
    {
        if (e.type == BattleSystem.ActionType.Tribute)
        {
            ChangeState(GetGameState(State.GameStateTribute));
        }

        else if (e.type == BattleSystem.ActionType.SelectUI)
        {
            ChangeState(GetGameState(State.GameStateWaiting));
        }

        else
        {
            ChangeState(previousGameState.Peek());
        }
    }

    private void BoxUI_OnBoxCancel(object sender, System.EventArgs e)
    {
        ChangeState(previousGameState.Peek());
    }

    private void BoxUI_OnBoxShow(object sender, System.EventArgs e)
    {
        ChangeState(GetGameState(State.GameStateWaiting));
    }

    private void TurnManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        havePlayedMonsterThatTurn = false;
    }

    protected virtual void Update()
    {

    }

    public override bool CanAction()
    {
        return canAction;
    }

    private IEnumerator PlayerDelay()
    {
        canAction = false;

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_ACTIONS);

        canAction = true;
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

                        card.GetCardVisual().FaceUpOnHand();

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

                card.GetCardVisual().FaceUpOnHand();

                yield return StartCoroutine(handZone.AddCard(card));

                yield return null;
            }
        }

        StartCoroutine(PlayerDelay());
    }

    protected override void SummonMonsterCardOnAttackPosition(MonsterCard monsterCard)
    {
        base.SummonMonsterCardOnAttackPosition(monsterCard);
    }

    protected override void SummonMonsterCardOnDefendPosition(MonsterCard monsterCard)
    {
        base.SummonMonsterCardOnDefendPosition(monsterCard);
    }

    public override IEnumerator PlayMonsterCardFromHand(MonsterCard monsterCard, MonsterZoneSingle zoneSlot)
    {
        yield return StartCoroutine(base.PlayMonsterCardFromHand(monsterCard, zoneSlot));

        monsterCard.GetMonsterZoneSingle().SetUpText();

        //yield return StartCoroutine(EffectsManager.Instance.ExecuteEffects());

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());

        yield return StartCoroutine(PlayerDelay());
    }

    public override IEnumerator SpecialSummon(MonsterCard monsterCard)
    {
        OnPlayerSpecialMonster?.Invoke(this, EventArgs.Empty);

        StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.EnemySpecialSummon));

        yield return StartCoroutine(base.SpecialSummon(monsterCard));

        monsterCard.GetMonsterZoneSingle().SetUpText();
    }

    public override IEnumerator RevealCardOnHand(Card card)
    {
        yield return StartCoroutine(base.RevealCardOnHand(card));
    }

    public override IEnumerator PlaySpellCardFromHand(SpellCard spellCard, SpellTrapZoneSingle zoneSlot)
    {
        yield return StartCoroutine(base.PlaySpellCardFromHand(spellCard, zoneSlot));

        if (spellCard.GetSpellCardData().spellCardState == SpellCardState.Faceup)
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.EnemyPlaySpellCard));
        }

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());
    }

    public override IEnumerator ActiveSpellCardOnField(SpellCard spellCard)
    {
        yield return StartCoroutine(base.ActiveSpellCardOnField(spellCard));

        yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.EnemyPlaySpellCard));

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());
    }
}
