using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    public static BattleState Instance { get; private set; }

    public event EventHandler OnStartBattle;

    public event EventHandler OnEndBattle;

    public event EventHandler OnChooseMonsterToAttack;

    //public event EventHandler OnAttackFinished;

    private Character characterAttack;

    private MonsterCard currentMonsterAttack;

    private MonsterCard currentMonsterBeAttacked;

    private List<MonsterCard> monstersToDisappear;

    private float timeDelay = 0.75f;

    private float timeFlip = 1f;

    private bool directAttack;

    private void Awake()
    {
        Instance = this;

        monstersToDisappear = new List<MonsterCard>();
    }

    private void Start()
    {
        SwordIconVisual.OnSwordReachTarget += SwordIconVisual_OnSwordReachTarget;

        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, EventArgs e)
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        SwordIconVisual.OnSwordReachTarget -= SwordIconVisual_OnSwordReachTarget;
    }

    private void SwordIconVisual_OnSwordReachTarget(object sender, EventArgs e)
    {
        StartCoroutine(BattleStep());
    }

    public bool CanChangeToBattleState()
    {
        Character currentTurn = TurnManager.Instance.GetCurrentTurn();

        List<MonsterCard> monstersAttackList = currentTurn.GetMonsterZone().GetMonsterCardsCanAttackOnField();

        return monstersAttackList.Count > 0;
    }

    public void SetCharacterAttack(Character character)
    {
        this.characterAttack = character;

        BattleSystem.Instance.SetCharacterAction(characterAttack);
    }

    public void ResetCharacterAttack()
    {
        this.characterAttack = null;
    }

    public void UndoAttack()
    {
        currentMonsterAttack.GetSwordIcon().GetSwordIconVisual().ResetSword();

        ResetCurrentMonsterAttack();

        ResetCurrentMonsterBeAttacked();

        TurnOffSelectableMonstersOnEnemyField();

        this.characterAttack.GetMonsterZone().GetToBattleState();
    }

    public void SetCurrentMonsterAttack(MonsterCard monsterCard)
    {
        SetCharacterAttack(Player.Instance);

        OnChooseMonsterToAttack?.Invoke(this, EventArgs.Empty);

        this.currentMonsterAttack = monsterCard;

        monsterCard.GetCardVisual().CardNormalStateOnField();

        if (TurnManager.Instance.GetNotCurrentTurn().GetMonsterZone().GetMonsterCardsOnField().Count == 0)
        {
            Character beAttacked = TurnManager.Instance.GetNotCurrentTurn();

            directAttack = true;

            monsterCard.GetSwordIcon().GetSwordIconVisual().RotateToPoint(beAttacked.GetAttackPoint().position);

            Player.Instance.ChangeState(Player.Instance.GetGameState(Character.State.GameStateWaiting));
        }

        else
        {
            monsterCard.GetSwordIcon().EnableRotateByMouse();

            directAttack = false;
        }

        TurnOnSelectableMonstersOnEnemyField();

        TurnOffSelectableMonstersToAttack();
    }
    
    public void AISetCurrentMonsterAttack(MonsterCard monsterAttack, MonsterCard monsterBeAttacked = null)
    {
        SetCharacterAttack(AI.Instance);

        this.currentMonsterAttack = monsterAttack;

        if (TurnManager.Instance.GetNotCurrentTurn().GetMonsterZone().GetMonsterCardsOnField().Count == 0)
        {
            Character beAttacked = TurnManager.Instance.GetNotCurrentTurn();

            directAttack = true;

            monsterAttack.GetSwordIcon().GetSwordIconVisual().RotateToPoint(beAttacked.GetAttackPoint().position);

            AI.Instance.ChangeState(AI.Instance.GetGameState(Character.State.GameStateWaiting));
        }

        else
        {
            directAttack = false;

            GetCurrentMonsterAttack().GetSwordIcon().GetSwordIconVisual().RotateToPoint(monsterBeAttacked.transform.position);

            SetCurrentMonsterBeAttacked(monsterBeAttacked);
        }
    }

    public MonsterCard GetCurrentMonsterAttack()
    {
        return currentMonsterAttack;
    }

    public void ResetCurrentMonsterAttack()
    {
        this.currentMonsterAttack = null;
    }

    public void SetCurrentMonsterBeAttacked(MonsterCard monsterCard)
    {
        this.currentMonsterBeAttacked = monsterCard;

        characterAttack.ChangeState(characterAttack.GetGameState(Character.State.GameStateWaiting));
    }

    public MonsterCard GetCurrentMonsterBeAttacked()
    {
        return currentMonsterBeAttacked;
    }

    public void ResetCurrentMonsterBeAttacked()
    {
        this.currentMonsterBeAttacked = null;
    }

    public void TurnOnSelectableMonstersOnEnemyField()
    {
        foreach (MonsterCard monster in TurnManager.Instance.GetNotCurrentTurn().GetMonsterZone().GetMonsterCardsOnField())
        {
            monster.GetEnemyMonsterAttackClick().TurnOnSelectable();
        }
    }

    public void TurnOffSelectableMonstersOnEnemyField()
    {
        foreach (MonsterCard monster in TurnManager.Instance.GetNotCurrentTurn().GetMonsterZone().GetMonsterCardsOnField())
        {
            monster.GetEnemyMonsterAttackClick().TurnOffSelectable();
        }
    }

    public void TurnOffSelectableMonstersToAttack()
    {
        foreach (MonsterCard monster in TurnManager.Instance.GetCurrentTurn().GetMonsterZone().GetMonsterCardsOnField())
        {
            if (monster != currentMonsterAttack)
            {
                monster.GetCardAttackOnField().TurnOffSelectable();
            }
        }
    }

    public void EndBattlePhase()
    {
        OnEndBattle?.Invoke(this, EventArgs.Empty);

        Character currentTurn = TurnManager.Instance.GetCurrentTurn();

        currentTurn.GetMonsterZone().ExitBattleState();

        TurnOffSelectableMonstersOnEnemyField();

        ResetCurrentMonsterAttack();

        ResetCurrentMonsterBeAttacked();

        ResetCharacterAttack();
    }

    public void StartStep()
    {
        OnStartBattle?.Invoke(this, EventArgs.Empty);

        Character currentTurn = TurnManager.Instance.GetCurrentTurn();

        currentTurn.GetMonsterZone().GetToBattleState();

        SetCharacterAttack(currentTurn);

        // fast effects
    }

    public IEnumerator BattleStep()
    {
        yield return new WaitForSeconds(timeDelay);

        yield return StartCoroutine(DeclareAttack());

        yield return StartCoroutine(DamageStep());
    }

    public void EndStep()
    {
        
    }

    public IEnumerator DeclareAttack()
    {
        // fast effects + counter

        // pop up windows

        yield break;
    }

    public IEnumerator DamageStep()
    {
        yield return StartCoroutine(StartDamageStep());
    }

    private IEnumerator StartDamageStep()
    {
        currentMonsterAttack.UpdateCardAttackState(CardAttackState.HaveAttacked);

        if (directAttack)
        {
            yield return StartCoroutine(DirectAttack(currentMonsterAttack));
        }

        else
        {
            yield return StartCoroutine(BeforeDamageCalculation());
        }
    }

    private IEnumerator BeforeDamageCalculation()
    {
        if (currentMonsterBeAttacked.GetMonsterCardData().cardPosition == CardPosition.DefendFacedown)
        {
            currentMonsterBeAttacked.UpdateCardPosition(CardPosition.DefendFaceup);

            yield return StartCoroutine(currentMonsterBeAttacked.GetCardVisual().FlipMonster());

            currentMonsterBeAttacked.GetMonsterZoneSingle().SetUpText();

            yield return new WaitForSeconds(timeFlip);
        }

        else if(currentMonsterBeAttacked.GetMonsterCardData().cardPosition == CardPosition.DefendFaceup)
        {

        }

        else
        {

        }

        // fast effects ATK DEF

        // pop up windows

        yield return StartCoroutine(DamageCalculation(currentMonsterAttack, currentMonsterBeAttacked));

        yield break;
    }

    private IEnumerator DamageCalculation(MonsterCard monsterAttack, MonsterCard monsterBeAttacked)
    {
        if (monsterBeAttacked.GetMonsterCardData().cardPosition == CardPosition.DefendFacedown || monsterBeAttacked.GetMonsterCardData().cardPosition == CardPosition.DefendFaceup)
        {
            yield return StartCoroutine(DamageCalculationBetweenTwoMonstersOnAttackPositionAndDefendPosition(monsterAttack, monsterBeAttacked));
        }

        else
        {
            yield return StartCoroutine(DamageCalculationBetweenTwoMonstersOnAttackPosition(monsterAttack, monsterBeAttacked));
        }

        yield return StartCoroutine(EndDamageStep());
    }

    private IEnumerator EndDamageStep()
    {
        foreach (MonsterCard monster in monstersToDisappear)
        {
            yield return StartCoroutine(monster.SetCardToGraveyard());
        }

        monstersToDisappear.Clear();

        yield return StartCoroutine(EffectsManager.Instance.ExecuteStackEffects());

        TurnManager.Instance.GetCurrentTurn().GetMonsterZone().GetToBattleState();

        characterAttack.ChangeState(characterAttack.GetGameState(Character.State.GameStateBattle));
    }

    private IEnumerator DirectAttack(MonsterCard monsterAttack)
    {
        MonsterCardData monsterAttackData = monsterAttack.GetMonsterCardData();

        Character beAttacked = TurnManager.Instance.GetNotCurrentTurn();

        yield return StartCoroutine(PointManager.Instance.DecreasePoint(monsterAttackData.attackValue, beAttacked.GetAttackPoint().position, beAttacked));

        yield return StartCoroutine(EndDamageStep());
    }

    private IEnumerator DamageCalculationBetweenTwoMonstersOnAttackPosition(MonsterCard monsterAttack, MonsterCard monsterBeAttacked)
    {
        MonsterCardData monsterAttackData = monsterAttack.GetMonsterCardData();

        MonsterCardData monsterBeAttackedData = monsterBeAttacked.GetMonsterCardData();

        if (monsterAttackData.attackValue > monsterBeAttackedData.attackValue)
        {
            monstersToDisappear.Add(monsterBeAttacked);

            StartCoroutine(monsterBeAttacked.GetCardVisual().CardBeAttacked());

            yield return StartCoroutine(PointManager.Instance.DecreasePoint(monsterAttackData.attackValue - monsterBeAttackedData.attackValue, monsterBeAttacked.transform.position, monsterBeAttacked.GetOwner()));
        }

        else if (monsterAttackData.attackValue < monsterBeAttackedData.attackValue)
        {
            monstersToDisappear.Add(monsterAttack);

            StartCoroutine(monsterAttack.GetCardVisual().CardBeAttacked());

            yield return StartCoroutine(PointManager.Instance.DecreasePoint(monsterBeAttackedData.attackValue - monsterAttackData.attackValue, monsterAttack.transform.position, monsterAttack.GetOwner()));
        }

        else
        {
            monstersToDisappear.Add(monsterAttack);

            monstersToDisappear.Add(monsterBeAttacked);

            StartCoroutine(monsterAttack.GetCardVisual().CardBeAttacked());

            StartCoroutine(monsterBeAttacked.GetCardVisual().CardBeAttacked());

            yield return StartCoroutine(PointManager.Instance.DecreasePoint(0, monsterAttack.transform.position, monsterAttack.GetOwner()));
        }
    }
    private IEnumerator DamageCalculationBetweenTwoMonstersOnAttackPositionAndDefendPosition(MonsterCard monsterAttack, MonsterCard monsterBeAttacked)
    {
        MonsterCardData monsterAttackData = monsterAttack.GetMonsterCardData();

        MonsterCardData monsterBeAttackedData = monsterBeAttacked.GetMonsterCardData();

        if (monsterAttackData.attackValue > monsterBeAttackedData.defendValue)
        {
            monstersToDisappear.Add(monsterBeAttacked);

            StartCoroutine(monsterBeAttacked.GetCardVisual().CardBeAttacked());

            yield return StartCoroutine(PointManager.Instance.DecreasePoint(0, monsterBeAttacked.transform.position, monsterBeAttacked.GetOwner()));
        }

        else if (monsterAttackData.attackValue < monsterBeAttackedData.defendValue)
        {

            StartCoroutine(monsterAttack.GetCardVisual().CardBeAttacked());

            yield return StartCoroutine(PointManager.Instance.DecreasePoint(monsterBeAttackedData.defendValue - monsterAttackData.attackValue, monsterAttack.transform.position, monsterAttack.GetOwner()));
        }

        else
        {
            yield return StartCoroutine(PointManager.Instance.DecreasePoint(0, monsterAttack.transform.position, monsterAttack.GetOwner()));
        }
    }
}
