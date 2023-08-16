using System;
using UnityEngine;

[CreateAssetMenu()]
public class MonsterCardSO : CardSO
{
    public int attackValue;

    public int defendValue;

    public int level;

    public ElementType elementType;

    public MonsterType monsterType;

    public EffectType effectType;
}