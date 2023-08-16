using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverOnHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float yOffset;

    [SerializeField] private float timeOffset;

    [SerializeField] private Card card;

    private bool isHovering;

    private bool isRevealing;

    private void Awake()
    {
        isHovering = false;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (isHovering && !isRevealing)
        {
            isHovering = false;

            CardNormalOnHand();

            CardInfoUI.Instance.ExitHovering();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering && !isRevealing)
        {
            isHovering = true;

            HoverCardOnHand();

            CardInfoUI.Instance.ShowCardInfo(card);
        }
    }

    public void HoverCardOnHand()
    {
        LeanTween.moveLocalY(gameObject, yOffset, timeOffset).setEase(LeanTweenType.easeOutQuad);
    }

    public void CardNormalOnHand()
    {
        LeanTween.moveLocalY(gameObject, 0, timeOffset).setEase(LeanTweenType.easeOutQuad);
    }

    public void TurnOffHovering()
    {
        isHovering = true;

        isRevealing = true;
    }

    public void TurnOnHovering()
    {
        isHovering = false;

        isRevealing = false;
    }
}
