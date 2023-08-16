using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayLifePoints : SpellCardEffect
{
    private int pointToPay;

    private void Awake()
    {
        
    }

    public override bool ConditionsToActive()
    {
        return owner.GetLifePoints() > pointToPay;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.pointToPay = (int)list[0];
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        yield return StartCoroutine(PointManager.Instance.DecreasePoint(pointToPay, owner.GetAttackPoint().position, owner));

        SetEffectResult(true);

        yield break;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
