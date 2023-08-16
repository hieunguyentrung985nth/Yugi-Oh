using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCardUIEffect : SpellCardEffect
{
    private List<Card> cardsList;

    private int numberToSelect;

    private List<Card> cardsSelect;

    private string textToShow;

    private TextBoxType textBoxType;

    private void Awake()
    {
        cardsList = new List<Card>();

        cardsSelect = new List<Card>();
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.cardsList = list[0] as List<Card>;

        this.textBoxType = (TextBoxType)Enum.Parse(typeof(TextBoxType), list[1].ToString());
    }

    public override bool ConditionsToActive()
    {
        return true;
    }

    public override IEnumerator Resolve()
    {
        card.Prepare();

        if (cardsList.Count != 0)
        {
            textToShow = TextDictionary.Instance.GetTextBox(textBoxType).success;
        }

        else
        {
            textToShow = TextDictionary.Instance.GetTextBox(textBoxType).failed;
        }

        BoxUI.Instacne.StartSelectCard(this, textToShow);

        while (!confirmAction)
        {
            yield return null;
        }

        SetEffectResult(false);

        if (cardsList.Count != 0)
        {
            StartCoroutine(BoxUI.Instacne.GetSelectBox().SetUp(cardsList));

            BattleSystem.Instance.SetActionType(BattleSystem.ActionType.SelectUI);

            SetEffectResult(true);
        }

        else
        {
            BattleSystem.Instance.ResetActionType();

            yield break;
        }

        BattleSystem.Instance.OnCarryActionEvent();

        while (cardsSelect.Count == 0)
        {
            yield return null;
        }

        BoxUI.Instacne.GetSelectBox().Hide();

        yield return new WaitForSeconds(0.5f);
    }

    public override void ResetValues()
    {
        cardsList.Clear();

        textToShow = default;

        confirmAction = false;

        base.ResetValues();
    }

    public override void SetCardSelect(Card card)
    {
        this.cardsSelect.Add(card);
    }

    public override List<Card> GetCardSelect()
    {
        return this.cardsSelect;
    }
}
