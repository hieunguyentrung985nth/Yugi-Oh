using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellCardPlayOnHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
            SpellCard spellCard = card as SpellCard;

            card.GetCardVisual().CardNormalStateOnHand();

            if (spellCard.GetCardStatus() == CardStatus.CanActive)
            {
                spellCard.UpdateSpellCardState(SpellCardState.Faceup);
            }

            else
            {
                spellCard.UpdateSpellCardState(SpellCardState.Facedown);
            }

            canPlay = false;

            isFaceup = false;

            TooltipManager.Instance.TurnOffTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canPlay)
        {
            SpellCard spellCard = card as SpellCard;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (spellCard.GetCardStatus() == CardStatus.CanActive)
                {
                    isFaceup = !isFaceup;

                    if (isFaceup)
                    {
                        spellCard.UpdateSpellCardState(SpellCardState.Faceup);
                    }

                    else
                    {
                        spellCard.UpdateSpellCardState(SpellCardState.Facedown);
                    }
                }

                else
                {
                    spellCard.UpdateSpellCardState(SpellCardState.Facedown);
                }

                BattleSystem.Instance.TooltipCardOnHandEvent(card);
            }

            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                CardPlayHandManager.Instance.PlaySpellCard(card as SpellCard, Player.Instance);

                canPlay = false;

                card.GetCardVisual().CardNormalStateOnHand();

                isHovering = false;

                TooltipManager.Instance.TurnOffTooltip();
            }
        }
    }
}
