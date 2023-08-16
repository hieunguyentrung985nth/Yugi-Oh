using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZone : MonoBehaviour
{
    [SerializeField] private Character owner;

    [SerializeField] private List<MonsterZoneSingle> monsterZoneSinglesList;

    private void Start()
    {

    }

    public MonsterZoneSingle GetEmptyMonsterSlot()
    {
        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (!single.HasAlreadyFull())
            {
                return single;
            }
        }

        return null;
    }

    public List<MonsterCard> GetMonsterCardsCanAttackOnField()
    {
        List<MonsterCard> monstersList = new List<MonsterCard>();

        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (single.HasAlreadyFull() && single.GetMonsterOnThisSlot().CanAttack())
            {
                monstersList.Add(single.GetMonsterOnThisSlot());
            }
        }

        return monstersList;
    }

    public List<MonsterCard> GetMonsterCardsOnField()
    {
        List<MonsterCard> monstersList = new List<MonsterCard>();

        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (single.HasAlreadyFull())
            {
                monstersList.Add(single.GetMonsterOnThisSlot());
            }
        }

        return monstersList;
    }

    public List<MonsterCard> GetMonsterTributeOnField()
    {
        List<MonsterCard> monstersList = new List<MonsterCard>();

        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (single.HasAlreadyFull())
            {
                monstersList.Add(single.GetMonsterOnThisSlot());
            }
        }

        return monstersList;
    }

    public bool HaveEmptyMonsterZoneSlotsOnField()
    {
        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (!single.HasAlreadyFull())
            {
                return true;
            }
        }

        return false;
    }

    public void GetToBattleState()
    {
        List<MonsterCard> monsterCards = GetMonsterCardsCanAttackOnField();

        foreach (MonsterCard monster in monsterCards)
        {
            monster.GetCardAttackOnField().TurnOnSelectable();

            monster.GetSwordIcon().ShowSwordIcon();
        }
    }

    public void ExitBattleState()
    {
        List<MonsterCard> monsterCards = GetMonsterCardsOnField();

        foreach (MonsterCard monster in monsterCards)
        {
            monster.GetCardAttackOnField().TurnOffSelectable();

            monster.GetSwordIcon().HideSwordIcon();
        }
    }

    public bool CheckIfOwnThisZone(MonsterZoneSingle monsterZoneSingle)
    {
        foreach (MonsterZoneSingle single in monsterZoneSinglesList)
        {
            if (single == monsterZoneSingle)
            {
                return true;
            }
        }

        return false;
    }
}
