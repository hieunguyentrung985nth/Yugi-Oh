using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterZoneSingle : MonoBehaviour
{
    [SerializeField] private TextMeshPro attackText;

    [SerializeField] private TextMeshPro slashText;

    [SerializeField] private TextMeshPro defendText;

    [SerializeField] private Color32 defaultColorText;

    [SerializeField] private Color32 highlightColorText;

    private MonsterCard monsterCard;

    private void Start()
    {
        HideText();
    }

    public void SetMonsterForThisSlot(MonsterCard monsterCard)
    {
        this.monsterCard = monsterCard;
    }

    public void ResetSlot()
    {
        this.monsterCard = null;
    }

    public bool HasAlreadyFull()
    {
        return monsterCard != null;
    }

    public MonsterCard GetMonsterOnThisSlot()
    {
        return monsterCard;
    }

    public void SetUpText()
    {
        MonsterCardData monsterCardData = monsterCard.GetMonsterCardData();

        attackText.text = monsterCardData.attackValue.ToString();

        defendText.text = monsterCardData.defendValue.ToString();

        HighlightTextBasedOnPosition(monsterCardData);

        ShowText();
    }

    private void HighlightTextBasedOnPosition(MonsterCardData monsterCardData)
    {
        if (monsterCardData.cardPosition == CardPosition.Attack)
        {
            attackText.color = highlightColorText;

            defendText.color = defaultColorText;
        }

        else
        {
            defendText.color = highlightColorText;

            attackText.color = defaultColorText;
        }
    }

    public void HideText()
    {
        attackText.gameObject.SetActive(false);

        defendText.gameObject.SetActive(false);

        slashText.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        attackText.gameObject.SetActive(true);

        defendText.gameObject.SetActive(true);

        slashText.gameObject.SetActive(true);
    }

    public bool IsOnOwnerZone()
    {
        return monsterCard.GetOwner().GetMonsterZone().CheckIfOwnThisZone(this);
    }
}
