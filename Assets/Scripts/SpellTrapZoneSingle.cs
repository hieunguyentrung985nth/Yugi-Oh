using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellTrapZoneSingle : MonoBehaviour
{
    [SerializeField] private TextMeshPro statusText;

    [SerializeField] private Color32 defaultColorText;

    [SerializeField] private Color32 highlightColorText;

    private SpellTrapCard spellTrapCard;

    private SpellCard spellCard;

    private TrapCard trapCard;

    private void Start()
    {
        HideText();
    }

    public void SetSpellTrapForThisSlot(SpellTrapCard spellTrapCard)
    {
        this.spellTrapCard = spellTrapCard;

        if (spellTrapCard is SpellCard)
        {
            this.spellCard = spellTrapCard as SpellCard;

            this.trapCard = null;
        }

        else
        {
            this.trapCard = spellTrapCard as TrapCard;

            this.spellCard = null;
        }
    }

    public void ResetSlot()
    {
        this.spellTrapCard = null;

        this.spellCard = null;

        this.trapCard = null;
    }

    public bool HasAlreadyFull()
    {
        return spellTrapCard != null;
    }

    public SpellTrapCard GetSpellTrapOnThisSlot()
    {
        return spellTrapCard;
    }

    public void SetUpText()
    {
        if (this.spellCard != null)
        {
            SpellCardData spellCardData = spellCard.GetSpellCardData();

            statusText.text = "1";
        }

        ShowText();
    }

    public void HideText()
    {
        statusText.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        statusText.gameObject.SetActive(true);
    }

    public bool IsOnOwnerZone()
    {
        return spellTrapCard.GetOwner().GetSpellTrapZone().CheckIfOwnThisZone(this);
    }
}
