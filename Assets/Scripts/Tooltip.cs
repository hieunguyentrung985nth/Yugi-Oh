using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("Content")]
    [SerializeField] private string summonText;

    [SerializeField] private string setText;

    [SerializeField] private string activeText;

    [SerializeField] private string attackText;

    [SerializeField] private string changeToAttackPositionText;

    [SerializeField] private string changeToDefendPositionText;

    [SerializeField] private string flipSummonText;

    [Header("Tooltip")]
    [SerializeField] private Color32 defaultColorText;

    [SerializeField] private Color32 activeColorText;

    [SerializeField] private Color32 defaultBackgroundColor;

    [SerializeField] private Color32 activeBackgroundColor;

    [SerializeField] private Color32 changeToAttackPositionColorText;

    [SerializeField] private Color32 changeToDefendPositionColorText;

    [SerializeField] private Color32 flipSummonBackgroundColor;

    [SerializeField] private Color32 flipSummonTextColor;

    [SerializeField] private Color32 attackTextColor;

    [SerializeField] private Color32 attackBackgroundColor;

    private Card card;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void CardOnHand(Card card)
    {
        this.card = card;

        if (card is MonsterCard)
        {
            SummonSet(card);
        }

        else if(card is SpellCard)
        {
            ActiveSet(card);
        }

        Show();
    }

    private void SummonSet(Card card)
    {
        background.color = defaultBackgroundColor;

        tooltipText.color = defaultColorText;

        MonsterCard monsterCard = card as MonsterCard;

        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.Attack)
        {
            tooltipText.text = summonText;
        }

        else
        {
            tooltipText.text = setText;
        }
    }

    private void ActiveSet(Card card)
    {
        background.color = defaultBackgroundColor;

        tooltipText.color = defaultColorText;

        SpellCard spellCard = card as SpellCard;

        if (spellCard.GetSpellCardData().spellCardState == SpellCardState.Faceup)
        {
            tooltipText.text = activeText;

            tooltipText.color = activeColorText;

            background.color = activeBackgroundColor;
        }

        else
        {
            tooltipText.text = setText;

            tooltipText.color = defaultColorText;

            background.color = defaultBackgroundColor;
        }
    }

    public void ChangePosition(Card card)
    {
        background.gameObject.SetActive(false);

        MonsterCard monsterCard = card as MonsterCard;

        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.Attack)
        {
            tooltipText.text = changeToDefendPositionText;

            tooltipText.color = changeToDefendPositionColorText;
        }

        else if(monsterCard.GetMonsterCardData().cardPosition == CardPosition.DefendFaceup)
        {
            tooltipText.text = changeToAttackPositionText;

            tooltipText.color = changeToAttackPositionColorText;
        }

        else
        {
            background.gameObject.SetActive(true);

            background.color = flipSummonBackgroundColor;

            tooltipText.text = flipSummonText;

            tooltipText.color = flipSummonTextColor;
        }

        Show();
    }

    public void Attack()
    {
        background.color = attackBackgroundColor;

        tooltipText.color = attackTextColor;

        tooltipText.text = attackText;

        Show();
    }

    public void Active()
    {
        background.color = activeBackgroundColor;

        tooltipText.color = activeColorText;

        tooltipText.text = activeText;

        Show();
    }

    private void Show()
    {
        transform.position = Input.mousePosition;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        background.gameObject.SetActive(true);
    }
}
