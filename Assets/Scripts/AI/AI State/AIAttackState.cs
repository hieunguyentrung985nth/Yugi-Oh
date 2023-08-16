using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    private MonsterCard monsterAttack;

    private MonsterCard monsterBeAttacked;

    public AIAttackState(AI aI) : base(aI)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        List<MonsterCard> monstersOnField = AI.Instance.GetMonsterZone().GetMonsterCardsCanAttackOnField();

        List<MonsterCard> enemyMonstersOnField = Player.Instance.GetMonsterZone().GetMonsterCardsOnField();

        if (monstersOnField.Count > 0)
        {
            monsterAttack = monstersOnField[0];

            if (enemyMonstersOnField.Count > 0)
            {
                monsterBeAttacked = enemyMonstersOnField[0];
            }

            else
            {
                monsterBeAttacked = null;
            }
        }

        else
        {
            aI.ChangeState(aI.GetAIState(AI.State.AIEndTurnState));

            monsterAttack = null;

            monsterBeAttacked = null;
        }
    }

    public override IEnumerator ExecuteState()
    {
        if (monsterAttack != null)
        {
            BattleState.Instance.AISetCurrentMonsterAttack(monsterAttack, monsterBeAttacked);

            aI.ChangeState(aI.GetAIState(AI.State.AIWaitingState));
        }

        yield return null;
    }

    public override void ExitState()
    {
        base.ExitState();

        monsterAttack = null;

        monsterBeAttacked = null;
    }
}
