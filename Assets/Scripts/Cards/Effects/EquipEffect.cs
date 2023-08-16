using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipEffect : SpellCardEffect
{
    private List<Card> cardEquipped;

    private void Awake()
    {
        cardEquipped = new List<Card>();
    }

    public override bool ConditionsToActive()
    {
        return true;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.cardEquipped = list[0] as List<Card>;
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        MonsterCard monsterCardEquipped = cardEquipped[0] as MonsterCard;

        monsterCardEquipped.AddEquipEffect(this);

        yield return StartCoroutine(card.GetCardVisual().EquipCard(monsterCardEquipped.transform));

        SetEffectResult(true);

        yield break;
    }

    public override void ResetValues()
    {
        cardEquipped.Clear();

        base.ResetValues();
    }

    public override IEnumerator RevertEffect()
    {
        if (cardEquipped != null)
        {
            foreach (MonsterCard card in cardEquipped.Cast<MonsterCard>())
            {
                card.RemoveEquipEffect(this);

                yield return StartCoroutine(card.SetCardToGraveyard());
            }
        }      
    }
}
