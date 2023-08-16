using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionDefault : MonoBehaviour
{
    protected Card card;

    protected Character owner;

    public virtual void Initialize(Card card, Character owner)
    {
        this.card = card;

        this.owner = owner;
    }

    public abstract bool Condtions();

    public abstract void SetUp(params object[] values);

    public virtual void ResetValues()
    {

    }
}
