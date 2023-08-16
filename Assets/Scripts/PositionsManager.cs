using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionsManager : MonoBehaviour
{
    public static PositionsManager Instance { get; private set; }

    private List<MonsterCard> monstersCanChange;

    private List<MonsterCard> monstersCantChange;

    private List<MonsterCard> monstersCantChangeByEffects;

    private void Awake()
    {
        Instance = this;

        monstersCanChange = new List<MonsterCard>();

        monstersCantChange = new List<MonsterCard>();

        monstersCantChangeByEffects = new List<MonsterCard>();
    }

    private void Start()
    {
        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        if (PhaseManager.Instance.IsMainPhase())
        {
            Character currentTurn = TurnManager.Instance.GetCurrentTurn();

            foreach (MonsterCard monster in currentTurn.GetMonsterZone().GetMonsterCardsOnField())
            {
                if (monster.CanChangePosition())
                {
                    AddMonsterCanChange(monster);
                }
            }

            TurnOnChangePosition();
        }

        else
        {
            TurnOffChangePosition();
        }
    }

    private void TurnManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        ResetMonsterCanChange();
    }

    public void TurnOnChangePosition()
    {
        foreach (MonsterCard monster in monstersCanChange)
        {
            if (monster.CanChangePosition())
            {
                monster.GetComponent<CardChangePosition>().TurnOnSelectable();
            }

            else
            {
                monster.GetComponent<CardChangePosition>().TurnOffSelectable();
            }
        }
    }

    public void TurnOffChangePosition()
    {
        foreach (MonsterCard monster in monstersCanChange)
        {
            monster.GetComponent<CardChangePosition>().TurnOffSelectable();
        }
    }

    public void AddMonsterCanChange(MonsterCard monsterCard)
    {
        monstersCanChange.Add(monsterCard);
    }

    public void RemoveMonsterCanChange(MonsterCard monsterCard)
    {
        monstersCanChange.Remove(monsterCard);
    }

    public void ResetMonsterCanChange()
    {
        Character currentTurn = TurnManager.Instance.GetNotCurrentTurn();

        foreach (MonsterCard monster in currentTurn.GetMonsterZone().GetMonsterCardsOnField())
        {
            monster.GetComponent<CardChangePosition>().TurnOffSelectable();
        }

        monstersCanChange.Clear();
    }

    public bool CheckIfMonsterCantChangeByEffect(MonsterCard monsterCard)
    {
        return monstersCantChangeByEffects.Contains(monsterCard);
    }
}
