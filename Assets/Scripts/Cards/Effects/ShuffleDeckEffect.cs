using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleDeckEffect : SpellCardEffect
{
    private List<Character> characters;

    private void Awake()
    {
        characters = new List<Character>();
    }

    public override bool ConditionsToActive()
    {
        return true;
    }

    public override void SetUp(params object[] values)
    {
        List<object> list = new List<object>(values);

        this.characters = list[0] as List<Character>;
    }

    public override IEnumerator Resolve()
    {
        SetEffectResult(false);

        if (characters.Count != 0)
        {
            foreach (Character character in characters)
            {
                yield return StartCoroutine(character.GetDeckZone().ShuffleDeckAnimation());
            }

            SetEffectResult(false);
        }

        yield break;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
