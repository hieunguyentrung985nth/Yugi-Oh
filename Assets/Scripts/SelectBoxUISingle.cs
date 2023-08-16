using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBoxUISingle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private const int TIME_FOR_SELECT_TARGET = 8;

    [SerializeField] private Image frontImage;

    [SerializeField] private Image backImage;

    [SerializeField] private Image barImage;

    [SerializeField] private Transform statusText;

    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private TextMeshProUGUI defendText;

    [SerializeField] private Color32 playerBarColor;

    [SerializeField] private Color32 enemyBarColor;

    [SerializeField] private Color32 selectColor;

    [SerializeField] private Color32 defaultColor;

    [SerializeField] private Transform glowTransform;

    private Card card;

    private bool selectable;

    public void SetUp(Card card)
    {
        this.card = card;

        CardSO cardSO = card.GetCardSO();

        frontImage.sprite = cardSO.image;

        if (card.GetOwner() == Player.Instance)
        {
            barImage.color = playerBarColor;
        }

        else
        {
            barImage.color = enemyBarColor;
        }

        if (card is MonsterCard)
        {
            SetText(cardSO as MonsterCardSO);
        }

        else
        {
            HideText();
        }

        CardNormalStateOnSelect();
    }

    private void SetText(MonsterCardSO monsterCardSO)
    {
        attackText.text = monsterCardSO.attackValue.ToString();

        defendText.text = monsterCardSO.defendValue.ToString();

        ShowText();
    }

    private void ShowText()
    {
        statusText.gameObject.SetActive(true);
    }

    private void HideText()
    {
        statusText.gameObject.SetActive(false);
    }

    public void TurnOnSelectable()
    {
        selectable = true;
    }

    public void TurnOffSelectable()
    {
        selectable = false;
    }

    public void CardActiveOnSelect()
    {
        glowTransform.gameObject.SetActive(true);
    }

    public void CardNormalStateOnSelect()
    {
        glowTransform.gameObject.SetActive(false);

        frontImage.color = defaultColor;
    }

    private void SwitchSelectTarget()
    {
        frontImage.color = frontImage.color == defaultColor ? selectColor : defaultColor;

        backImage.color = backImage.color == defaultColor ? selectColor : defaultColor;
    }

    public IEnumerator SelectTargetOnSelect()
    {
        int currentTime = 0;

        float transitionTimerMax = 0.05f;

        float transitionTimer = 0f;

        SwitchSelectTarget();

        while (currentTime < TIME_FOR_SELECT_TARGET)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer >= transitionTimerMax)
            {
                transitionTimer = 0f;

                currentTime++;

                SwitchSelectTarget();
            }

            yield return null;
        }

        CardNormalStateOnSelect();

        BoxUI.Instacne.GetCurrentEffect().SetCardSelect(card);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectable && eventData.button == PointerEventData.InputButton.Left)
        {
            CardActiveOnSelect();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectable)
        {
            CardNormalStateOnSelect();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable)
        {
            selectable = false;

            SelectBoxUI.Instance.TurnOffSelectable();

            StartCoroutine(SelectTargetOnSelect());
        }
    }
}
