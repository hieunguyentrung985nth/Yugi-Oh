using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardSO : ScriptableObject
{
    public string cardName;

    [TextArea(3,5)]
    public string description;

    public Sprite image;

    public CardType cardType;

    public Card cardPrefab;

    public SpellSpeedType spellSpeedType;

    public virtual void Play()
    {

    }
}
