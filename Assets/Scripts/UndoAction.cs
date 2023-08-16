using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UndoAction : MonoBehaviour
{
    public static UndoAction Instance { get; private set; }

    public enum UndoActionState
    {
        Tribute,
        Attack
    }

    //public event EventHandler<OnUndoActionEventArgs> OnUndoAction;

    public class OnUndoActionEventArgs : EventArgs
    {
        public UndoActionState state;
    }

    private UndoActionState state;

    private bool canUndo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BattleSystem.Instance.OnCarryAction += BattleSystem_OnCarryAction;

        BattleState.Instance.OnChooseMonsterToAttack += BattleState_OnChooseMonsterToAttack;

        BoxUI.Instacne.OnBoxCancel += BoxUI_OnBoxCancel;
    }

    private void BattleState_OnChooseMonsterToAttack(object sender, EventArgs e)
    {
        SetUndoState(UndoActionState.Attack);

        canUndo = true;
    }

    private void Update()
    {
        TriggerUndo();
    }

    private void BattleSystem_OnCarryAction(object sender, System.EventArgs e)
    {
        if (BattleSystem.Instance.GetActionType() == BattleSystem.ActionType.SelectUI || BattleSystem.Instance.GetActionType() == default)
        {
            canUndo = false;
        }

        else
        {
            canUndo = true;
        }
    }

    private void BoxUI_OnBoxCancel(object sender, System.EventArgs e)
    {
        canUndo = false;
    }

    public UndoActionState GetUndoActionState()
    {
        return state;
    }

    public void TurnOffCanUndo()
    {
        canUndo = false;
    }

    public void SetUndoState(UndoActionState state)
    {
        this.state = state;
    }

    public void ResetUndoState()
    {
        this.state = default;
    }

    private void TriggerUndo()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (canUndo)
            {
                if (state == UndoActionState.Tribute)
                {
                    BoxUI.Instacne.ShowBoxAgain();
                }
                
                else
                {

                }

                canUndo = false;

                Player.Instance.UndoState();

                HandleUndo();
            }           
        }      
    }

    private void HandleUndo()
    {
        switch (state)
        {
            case UndoActionState.Tribute:

                TributeManager.Instance.BackToNormalState();

                break;

            case UndoActionState.Attack:

                BattleState.Instance.UndoAttack();

                break;

            default:
                break;
        }
    }
}
