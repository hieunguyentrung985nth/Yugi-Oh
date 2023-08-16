using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TributeManager;

public class CardPlayHandManager : MonoBehaviour
{
    public static CardPlayHandManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMonsterCard(MonsterCard monsterCard, Character character)
    {
        if (!monsterCard.NeedTribute())
        {
            character.PlayCardFromHand(monsterCard);

            return;
        }

        else
        {
            TributeManager.Instance.TriggerTribute(monsterCard, Player.Instance);
        }
    }

    public void PlaySpellCard(SpellCard spellCard, Character character)
    {
        character.PlayCardFromHand(spellCard);
    }

    public void AIPlayMonsterCard(MonsterCard monsterCard)
    {
        if (!monsterCard.NeedTribute())
        {
            AI.Instance.PlayCardFromHand(monsterCard);

            return;
        }

        else
        {
            TributeManager.Instance.AITriggerTribute(monsterCard);
        }
    }
}
