using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyStormEffect : SpellTrapDefault
{
    private List<Card> cards;

    private void Awake()
    {
        cards = new List<Card>();
    }

    private void Start()
    {

    }

    public override void InitializeResolveEffect(SpellCardEffect effect = null, params object[] values)
    {
        base.InitializeResolveEffect(GetResolveCardEffects()[0]);
    }
}
