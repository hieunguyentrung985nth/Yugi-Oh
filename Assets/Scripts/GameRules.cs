using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public static GameRules Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckCardCanPlay(Card card, Character character)
    {
        if (card is MonsterCard)
        {
            return CanPlayMonsterCard(card as MonsterCard, character);
        }

        else if(card is SpellCard)
        {
            return CanPlaySpellCard(card as SpellCard, character);
        }

        else if(card is TrapCard)
        {
            return CanPlayTrapCard(card as TrapCard);
        }

        return false;
    }

    private bool CanPlayMonsterCard(MonsterCard monsterCard, Character character)
    {
        if (PhaseManager.Instance.IsMainPhase()
            && !TurnManager.Instance.GetCurrentTurn().HavePlayedMonsterThatTurn()
            && monsterCard.IsCardOnHand()
            && TurnManager.Instance.GetCurrentTurn().CanAction()
            && character.PlayerCardPlayOnHandEnabled)
        {
            if (monsterCard.CheckTribute())
            {
                monsterCard.UpdateCardStatus(CardStatus.CanPlay);

                return true;
            }
        }

        monsterCard.UpdateCardStatus(CardStatus.CantPlay);

        return false;
    }

    private bool CanPlaySpellCard(SpellCard spellCard, Character character)
    {
        if (spellCard.IsCardOnHand()
            && TurnManager.Instance.GetCurrentTurn().CanAction()
            && character.PlayerCardPlayOnHandEnabled
            && PhaseManager.Instance.IsMainPhase()
            && character.GetSpellTrapZone().GetEmptySpellTrapSlot()
            && spellCard.IsCardOnHand())
        {
            if (spellCard.CanActiveCard())
            {
                spellCard.UpdateCardStatus(CardStatus.CanActive);

                spellCard.UpdateSpellCardState(SpellCardState.Faceup);

                return true;
            }

            spellCard.UpdateCardStatus(CardStatus.CanPlay);

            spellCard.UpdateSpellCardState(SpellCardState.Facedown);

            return true;
        }

        spellCard.UpdateCardStatus(CardStatus.CantPlay);

        return false;
    }

    private bool CanPlayTrapCard(TrapCard trapCard)
    {
        return true;
    }
}
