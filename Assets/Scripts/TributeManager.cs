using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TributeManager : MonoBehaviour
{
    public static TributeManager Instance { get; private set; }

    public event EventHandler<OnTributeStartEventArgs> OnTributeStart;

    public class OnTributeStartEventArgs : EventArgs
    {
        public int numberForTribute;
    }

    private List<MonsterCard> monstersForTribute;

    private List<MonsterCard> monstersMarkAsTribute;

    private int numberForTribute;

    private MonsterCard monsterToSummon;

    private Character character;

    private void Awake()
    {
        Instance = this;

        monstersForTribute = new List<MonsterCard>();

        monstersMarkAsTribute = new List<MonsterCard>();
    }

    private void Start()
    {
        TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;
    }

    private void TurnManager_OnChangeTurn(object sender, EventArgs e)
    {
        ResetMonstersForTributeList();

        CreateMonstersForTributeList();
    }

    public void TriggerTribute(MonsterCard monsterCard, Character character)
    {
        this.character = character;

        numberForTribute = monsterCard.NumberForTribute();

        monsterToSummon = monsterCard;

        BattleSystem.Instance.SetActionType(BattleSystem.ActionType.Tribute);

        BattleSystem.Instance.SetCharacterAction(this.character);

        OnTributeStart?.Invoke(this, new OnTributeStartEventArgs
        {
            numberForTribute = numberForTribute
        });
    }

    public void AITriggerTribute(MonsterCard monsterCard)
    {
        this.character = AI.Instance;

        numberForTribute = monsterCard.NumberForTribute();

        monsterToSummon = monsterCard;

        BattleSystem.Instance.SetActionType(BattleSystem.ActionType.Tribute);

        BattleSystem.Instance.SetCharacterAction(this.character);

        for (int i = 0; i < numberForTribute; i++)
        {
            foreach (MonsterCard monsterOnField in monstersForTribute)
            {
                if (!monstersMarkAsTribute.Contains(monsterOnField))
                {
                    if (monsterCard.GetMonsterCardData().attackValue > monsterOnField.GetMonsterCardData().attackValue)
                    {
                        AIMarkAsTribute(monsterOnField);

                        break;
                    }
                }              
            }           
        }
    }

    public void CreateMonstersForTributeList()
    {
        Character currentTurn = TurnManager.Instance.GetCurrentTurn();

        foreach (MonsterCard monster in currentTurn.GetMonsterZone().GetMonsterCardsOnField())
        {
            monstersForTribute.Add(monster);
        }
    }

    public void AddMonsterToTributeList(MonsterCard monsterCard)
    {
        monstersForTribute.Add(monsterCard);
    }

    public void RemoveMonsterFromTributeList(MonsterCard monsterCard)
    {
        monstersForTribute.Remove(monsterCard);
    }

    public void ResetMonstersForTributeList()
    {
        monstersForTribute.Clear();

        numberForTribute = 0;

        monstersMarkAsTribute.Clear();

        monsterToSummon = null;

        character = null;
    }

    public void BackToNormalState()
    {
        foreach (MonsterCard monster in monstersMarkAsTribute)
        {
            monster.GetCardVisual().CardNormalStateOnField();
        }

        monstersMarkAsTribute.Clear();
    }

    public List<MonsterCard> GetMonstersForTributeList()
    {
        return monstersForTribute;
    }

    public void TurnOnSelectableForTribute()
    {
        foreach (MonsterCard monster in monstersForTribute)
        {
            monster.GetComponent<CardTribute>().TurnOnSelectable();
        }
    }

    public void TurnOffSelectableForTribute()
    {
        foreach (MonsterCard monster in monstersForTribute)
        {
            monster.GetComponent<CardTribute>().TurnOffSelectable();
        }
    }

    public IEnumerator MarkAsTribute(MonsterCard monsterCard)
    {
        monsterCard.GetCardVisual().MarkAsTribute();

        monsterCard.GetComponent<CardTribute>().TurnOffSelectable();

        monstersMarkAsTribute.Add(monsterCard);

        if (monstersMarkAsTribute.Count == numberForTribute)
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.EnemySummonMonster));

            yield return StartCoroutine(Summon());
        }
    }

    public void AIMarkAsTribute(MonsterCard monsterCard)
    {
        monstersMarkAsTribute.Add(monsterCard);

        if (monstersMarkAsTribute.Count == numberForTribute)
        {
            StartCoroutine(Summon());
        }
    }

    private IEnumerator Summon()
    {
        MonsterZoneSingle zoneSlot = null;

        UndoAction.Instance.TurnOffCanUndo();

        for (int i = 0; i < monstersMarkAsTribute.Count; i++)
        {
            if (monstersMarkAsTribute[i].GetMonsterZoneSingle().IsOnOwnerZone() && zoneSlot == null)
            {
                zoneSlot = monstersMarkAsTribute[i].GetMonsterZoneSingle();
            }

            yield return StartCoroutine(monstersMarkAsTribute[i].GetCardVisual().Tribute());

            yield return StartCoroutine(monstersMarkAsTribute[i].Tribute());
        }

        yield return StartCoroutine(TurnManager.Instance.GetCurrentTurn().PlayMonsterCardFromHand(monsterToSummon, zoneSlot));

        ResetMonstersForTributeList();
    }
}
