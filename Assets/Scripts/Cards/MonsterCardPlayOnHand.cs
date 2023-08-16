using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MonsterCardPlayOnHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Card card;

    private bool canPlay;

    private bool isFaceup;

    private bool isHovering;

    private void Update()
    {
        HandleHovering();
    }

    private void HandleHovering()
    {
        if (isHovering)
        {
            if (GameRules.Instance.CheckCardCanPlay(card, Player.Instance))
            {
                card.GetCardVisual().CardCanBePlayedOnHand();

                canPlay = true;

                isFaceup = true;

                isHovering = false;

                BattleSystem.Instance.TooltipCardOnHandEvent(card);
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        if (GameRules.Instance.CheckCardCanPlay(card, Player.Instance))
        {
            card.GetCardVisual().CardCanBePlayedOnHand();

            canPlay = true;

            isFaceup = true;

            BattleSystem.Instance.TooltipCardOnHandEvent(card);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (canPlay)
        {
            card.GetCardVisual().CardNormalStateOnHand();

            (card as MonsterCard).UpdateCardPosition(CardPosition.Attack);

            canPlay = false;

            isFaceup = true;

            TooltipManager.Instance.TurnOffTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canPlay)
        {
            MonsterCard monsterCard = card as MonsterCard;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                isFaceup = !isFaceup;

                if (isFaceup)
                {
                    monsterCard.UpdateCardPosition(CardPosition.Attack);
                }

                else
                {
                    monsterCard.UpdateCardPosition(CardPosition.DefendFacedown);
                }

                BattleSystem.Instance.TooltipCardOnHandEvent(card);
            }

            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                CardPlayHandManager.Instance.PlayMonsterCard(monsterCard, Player.Instance);

                canPlay = false;

                card.GetCardVisual().CardNormalStateOnHand();

                isHovering = false;

                TooltipManager.Instance.TurnOffTooltip();
            }
        }
    }
}
