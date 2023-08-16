using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHoverOnField : MonoBehaviour
{
    [SerializeField] private Card card;

    private bool isOnHand;

    private void Start()
    {
        card.OnCardStateChange += Card_OnCardStateChange;

        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        isOnHand = true;
    }

    private void Card_OnCardStateChange(object sender, Card.OnCardStateChangeEventArgs e)
    {
        if (e.currentState == CardState.Hand)
        {
            isOnHand = true;
        }

        else
        {
            isOnHand = false;
        }
    }

    private void OnMouseEnter()
    {
        if (!isOnHand)
        {
            CardInfoUI.Instance.ShowCardInfo(card);
        }
    }

    private void OnMouseExit()
    {
        if (!isOnHand)
        {
            CardInfoUI.Instance.ExitHovering();
        }
    }
}
