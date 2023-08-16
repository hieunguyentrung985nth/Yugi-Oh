using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class MonsterCard : Card
{
    private const int LEVEL_4 = 4;

    private const int LEVEL_5 = 5;

    private const int LEVEL_6 = 6;

    private const int LEVEL_7 = 7;

    private const int TRIBUTE_NUMBER_FOR_LEVEL_4 = 0;

    private const int TRIBUTE_NUMBER_FOR_LEVEL_5_6 = 1;

    private const int TRIBUTE_NUMBER_FOR_LEVEL_7 = 2;

    public static event EventHandler<OnMonsterCardDestroyedEventArgs> OnMonsterCardDestroyed;

    public class OnMonsterCardDestroyedEventArgs : EventArgs
    {
        public MonsterCard monsterCard;
    }

    [SerializeField] private SwordIcon swordIcon;

    private MonsterCardData monsterCardData;

    private MonsterZoneSingle monsterZoneSingle;

    private CardAttackOnField cardAttackOnField;

    private EnemyMonsterAttackClick enemyMonsterAttackClick;

    [SerializeField] private SpellTrapDefault cardEffectObject;

    private Transform effectTransform;

    private SpellTrapDefault spellTrapDefault;

    private List<SpellCardEffect> spellCardEffect;

    private List<ConditionDefault> conditionCardEffect;

    private List<EquipEffect> equipEffects;

    private void Awake()
    {
        cardAttackOnField = GetComponent<CardAttackOnField>();

        enemyMonsterAttackClick = GetComponent<EnemyMonsterAttackClick>();

        equipEffects = new List<EquipEffect>();
    }

    protected override void Start()
    {
        base.Start();

        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        effectTransform = GameManager.Instance.GetEffectTransform();

        if (cardEffectObject != null)
        {
            InitializeEffect();
        }

        InitializeCard();
    }

    private void InitializeEffect()
    {
        spellTrapDefault = Instantiate(cardEffectObject, effectTransform);

        spellTrapDefault.Initialized();

        spellTrapDefault.SetUp(this, owner);

        spellTrapDefault.InitializeResolveEffect();

        spellTrapDefault.InitializeConditionEffect();

        spellTrapDefault.InitializeActivationEffect();

        spellCardEffect = spellTrapDefault.GetResolveCardEffects();

        conditionCardEffect = spellTrapDefault.GetConditionCardEffects();

        foreach (SpellCardEffect effect in spellCardEffect)
        {
            effect.Initialize(this, owner);
        }

        foreach (ConditionDefault condtion in conditionCardEffect)
        {
            condtion.Initialize(this, owner);
        }
    }

    public SpellTrapDefault GetSpellTrapDefault()
    {
        return spellTrapDefault;
    }

    private void TurnManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        UpdateCardAttackState(CardAttackState.CanAttack);

        if (!PositionsManager.Instance.CheckIfMonsterCantChangeByEffect(this))
        {
            TurnOnCanChangePosition();
        }

        else
        {
            TurnOffCanChangePosition();
        }
    }

    public void InitializeCard()
    {
        MonsterCardSO monsterCardSO = cardSO as MonsterCardSO;

        monsterCardData = new MonsterCardData(monsterCardSO);
    }
    
    public MonsterCardData GetMonsterCardData()
    {
        return monsterCardData;
    }

    public void TurnOffCanChangePosition()
    {
        monsterCardData.canChangePosition = false;
    }

    public void TurnOnCanChangePosition()
    {
        monsterCardData.canChangePosition = true;
    }

    public override bool CanPlayCard()
    {
        return cardData.cardStatus == CardStatus.CanPlay;
    }

    public bool CheckTribute()
    {
        int numberForTribute = NumberForTribute();

        int monstersOnFieldForTribute = TributeManager.Instance.GetMonstersForTributeList().Count;

        return monstersOnFieldForTribute >= numberForTribute;

        //if ()
        //{
        //    return true;
        //}

        //else if (monstersOnFieldForTribute >= TRIBUTE_NUMBER_FOR_LEVEL_7)
        //{
        //    return true;
        //}

        //else if(numberForTribute == TRIBUTE_NUMBER_FOR_LEVEL_4)
        //{
        //    if (owner.GetMonsterZone().HaveEmptyMonsterZoneSlotsOnField())
        //    {
        //        return true;
        //    }
        //}

        //return false;
    }

    public SwordIcon GetSwordIcon()
    {
        return swordIcon;
    }

    public void UpdateCardPosition(CardPosition cardPosition)
    {
        monsterCardData.cardPosition = cardPosition;
    }

    public void SetMonsterZoneSingle(MonsterZoneSingle monsterZoneSingle)
    {
        this.monsterZoneSingle = monsterZoneSingle;
    }

    public void ClearMonsterZoneSingle()
    {
        this.monsterZoneSingle = null;
    }

    public MonsterZoneSingle GetMonsterZoneSingle()
    {
        return monsterZoneSingle;
    }

    public bool CanAttack()
    {
        return monsterCardData.cardAttackState == CardAttackState.CanAttack && monsterCardData.cardPosition == CardPosition.Attack;
    }

    public CardAttackOnField GetCardAttackOnField()
    {
        return cardAttackOnField;
    }

    public EnemyMonsterAttackClick GetEnemyMonsterAttackClick()
    {
        return enemyMonsterAttackClick;
    }

    public void UpdateCardAttackState(CardAttackState state)
    {
        monsterCardData.cardAttackState = state;
    }

    public override IEnumerator SendCardToGraveyard()
    {
        GetMonsterZoneSingle().HideText();

        GetMonsterZoneSingle().ResetSlot();

        DefaultStateOnField();

        yield return StartCoroutine(base.SendCardToGraveyard());
    }

    public override void DefaultStateOnField()
    {
        UpdateCardAttackState(CardAttackState.CanAttack);

        UpdateCardPosition(CardPosition.Attack);

        UpdateCardStatus(CardStatus.CantActive);

        TurnOffCanChangePosition();

        GetCardAttackOnField().TurnOffSelectable();

        GetSwordIcon().HideSwordIcon();
    }

    public bool CanChangePosition()
    {
        return monsterCardData.canChangePosition;
    }

    public void Flip()
    {
        // flip...
    }

    public int NumberForTribute()
    {
        if (monsterCardData.level <= LEVEL_4)
        {
            return TRIBUTE_NUMBER_FOR_LEVEL_4;
        }

        else if (LEVEL_5 <= monsterCardData.level && monsterCardData.level <= LEVEL_6)
        {
            return TRIBUTE_NUMBER_FOR_LEVEL_5_6;
        }

        else
        {
            return TRIBUTE_NUMBER_FOR_LEVEL_7;
        }
    }

    public bool NeedTribute()
    {
        return NumberForTribute() >= TRIBUTE_NUMBER_FOR_LEVEL_5_6;
    }

    public override IEnumerator SetCardToGraveyard()
    {
        GetMonsterZoneSingle().HideText();

        GetMonsterZoneSingle().ResetSlot();

        yield return StartCoroutine(base.SetCardToGraveyard());

        UpdateCardState(CardState.Graveyard);

        GetCardVisual().DefaultCardOnField();

        DefaultStateOnField();
    }

    public override IEnumerator DestroyCard()
    {
        OnMonsterCardDestroyed?.Invoke(this, new OnMonsterCardDestroyedEventArgs
        {
            monsterCard = this
        });

        yield return StartCoroutine(base.DestroyCard());

        yield return StartCoroutine(DestroyAllEquipEffects());

        TributeManager.Instance.RemoveMonsterFromTributeList(this);

        if ((GetCardSO() as MonsterCardSO).effectType == EffectType.Trigger)
        {
            EffectsManager.Instance.AddStackEffect(GetSpellTrapDefault());
        }
    }

    public override IEnumerator Tribute()
    {
        GetMonsterZoneSingle().HideText();

        GetMonsterZoneSingle().ResetSlot();

        yield return StartCoroutine(base.Tribute());

        TributeManager.Instance.RemoveMonsterFromTributeList(this);

        yield return StartCoroutine(DestroyAllEquipEffects());

        if ((GetCardSO() as MonsterCardSO).effectType == EffectType.Trigger)
        {
            EffectsManager.Instance.AddStackEffect(GetSpellTrapDefault());
        }
    }

    public bool CanActiveCard()
    {
        spellTrapDefault.Prepare();

        return spellTrapDefault.ConditionsForMonster();
    }

    public override void Prepare()
    {
        spellTrapDefault.Prepare();
    }

    public void AddEquipEffect(EquipEffect equipEffect)
    {
        this.equipEffects.Add(equipEffect);
    }

    public void RemoveEquipEffect(EquipEffect equipEffect)
    {
        this.equipEffects.Remove(equipEffect);
    }

    public IEnumerator DestroyAllEquipEffects()
    {
        foreach (EquipEffect effect in equipEffects)
        {
            effect.ResetValues();

            yield return StartCoroutine(effect.GetCard().SetCardToGraveyard());
        }
    }

    public override bool IsFaceUp()
    {
        return (monsterCardData.cardPosition == CardPosition.Attack || monsterCardData.cardPosition == CardPosition.DefendFaceup) && cardData.cardState != CardState.Hand;
    }
}
