using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellTrapCard : Card
{
    private SpellTrapZoneSingle zoneSingle;

    protected override void Start()
    {
        base.Start();
    }

    public abstract override bool CanPlayCard();

    public abstract override void DefaultStateOnField();

    public void SetSpellTrapZoneSingle(SpellTrapZoneSingle single)
    {
        this.zoneSingle = single;
    }

    public SpellTrapZoneSingle GetSpellTrapZoneSingle()
    {
        return zoneSingle;
    }
}
