using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterCardData
{
    public int defendValue;

    public int attackValue;

    public int level;

    public bool canChangePosition;

    public bool canAttack;

    public CardPosition cardPosition;

    public CardAttackState cardAttackState;

    public MonsterCardData(MonsterCardSO monsterCardSO)
    {
        this.defendValue = monsterCardSO.defendValue;

        this.attackValue = monsterCardSO.attackValue;

        this.level = monsterCardSO.level;

        this.canChangePosition = false;

        this.canAttack = true;

        this.cardPosition = CardPosition.Attack;

        cardAttackState = CardAttackState.CanAttack;
    }
}
