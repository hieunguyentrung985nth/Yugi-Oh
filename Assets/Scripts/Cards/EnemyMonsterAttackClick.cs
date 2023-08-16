using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMonsterAttackClick : MonoBehaviour
{
    private MonsterCard monsterCard;

    private bool selectable;

    public void TurnOffSelectable()
    {
        selectable = false;
    }

    public void TurnOnSelectable()
    {
        selectable = true;
    }

    private void Awake()
    {
        monsterCard = GetComponent<MonsterCard>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        TurnOffSelectable();
    }

    private void OnMouseEnter()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            monsterCard.GetCardVisual().CardActiveOnField();
        }
    }

    private void OnMouseExit()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            monsterCard.GetCardVisual().CardNormalStateOnField();
        }
    }

    private void OnMouseDown()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            Player.Instance.ChangeState(Player.Instance.GetGameState(Character.State.GameStateWaiting));

            TurnOffSelectable();

            monsterCard.GetCardVisual().CardNormalStateOnField();

            BattleState.Instance.GetCurrentMonsterAttack().GetSwordIcon().GetSwordIconVisual().RotateToPoint(transform.position);

            BattleState.Instance.TurnOffSelectableMonstersOnEnemyField();

            BattleState.Instance.SetCurrentMonsterBeAttacked(monsterCard);

            UndoAction.Instance.TurnOffCanUndo();
        }
    }
}
