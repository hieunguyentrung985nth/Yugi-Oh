using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public CardStatus cardStatus;

    public CardState cardState;

    public CardData()
    {
        this.cardStatus = CardStatus.CantPlay;

        this.cardState = CardState.Deck;
    }
}
