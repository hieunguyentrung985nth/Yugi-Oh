using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public List<SpellCardEffect> GetResolveCardEffects();

    public void Prepare();

    public void ClearEffect();
}
