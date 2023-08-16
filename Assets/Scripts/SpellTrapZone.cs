using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTrapZone : MonoBehaviour
{
    [SerializeField] private Character owner;

    [SerializeField] private List<SpellTrapZoneSingle> spellZoneSinglesList;

    public SpellTrapZoneSingle GetEmptySpellTrapSlot()
    {
        foreach (SpellTrapZoneSingle single in spellZoneSinglesList)
        {
            if (!single.HasAlreadyFull())
            {
                return single;
            }
        }

        return null;
    }

    public List<SpellTrapCard> GetSpellTrapCardsOnField()
    {
        List<SpellTrapCard> spellTrapList = new List<SpellTrapCard>();

        foreach (SpellTrapZoneSingle single in spellZoneSinglesList)
        {
            if (single.HasAlreadyFull())
            {
                spellTrapList.Add(single.GetSpellTrapOnThisSlot());
            }
        }

        return spellTrapList;
    }

    public List<SpellCard> GetSpellCardsOnField()
    {
        List<SpellCard> spellList = new List<SpellCard>();

        foreach (SpellTrapZoneSingle single in spellZoneSinglesList)
        {
            if (single.HasAlreadyFull() && single.GetSpellTrapOnThisSlot() is SpellCard)
            {
                spellList.Add(single.GetSpellTrapOnThisSlot() as SpellCard);
            }
        }

        return spellList;
    }

    public List<TrapCard> GetTrapCardsOnField()
    {
        List<TrapCard> trapList = new List<TrapCard>();

        foreach (SpellTrapZoneSingle single in spellZoneSinglesList)
        {
            if (single.HasAlreadyFull() && single.GetSpellTrapOnThisSlot() is TrapCard)
            {
                trapList.Add(single.GetSpellTrapOnThisSlot() as TrapCard);
            }
        }

        return trapList;
    }

    public bool CheckIfOwnThisZone(SpellTrapZoneSingle spellTrapZoneSingle)
    {
        foreach (SpellTrapZoneSingle single in spellZoneSinglesList)
        {
            if (single == spellTrapZoneSingle)
            {
                return true;
            }
        }

        return false;
    }
}
