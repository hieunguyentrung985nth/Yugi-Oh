using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public event EventHandler OnChangeTurn;

    public event EventHandler OnStartGame;

    public enum PlayerTurn
    {
        Player,
        AI
    }

    private PlayerTurn currentPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChooseTurnUI.Instance.OnChooseTurn += ChooseTurnUI_OnChooseTurn;

        PhaseManager.Instance.OnEndPhase += PhaseManager_OnEndPhase;
    }

    private void PhaseManager_OnEndPhase(object sender, EventArgs e)
    {
        ChangeTurn();
    }

    private void ChooseTurnUI_OnChooseTurn(object sender, ChooseTurnUI.OnChooseTurnEventArgs e)
    {
        if (e.isPlayerGoFirst)
        {
            currentPlayer = PlayerTurn.Player;
        }

        else
        {
            currentPlayer = PlayerTurn.AI;
        }

        OnStartGame?.Invoke(this, EventArgs.Empty);
    }

    private void ChangeTurn()
    {
        currentPlayer = currentPlayer == PlayerTurn.Player ? PlayerTurn.AI : PlayerTurn.Player;

        OnChangeTurn?.Invoke(this, EventArgs.Empty);
    }

    public Character GetCurrentTurn()
    {
        return currentPlayer == PlayerTurn.Player ? Player.Instance : AI.Instance;
    }

    public Character GetNotCurrentTurn()
    {
        return currentPlayer == PlayerTurn.Player ? AI.Instance : Player.Instance;
    }

    public bool IsPlayerTurn()
    {
        return currentPlayer == PlayerTurn.Player;
    }

    public bool IsAITurn()
    {
        return currentPlayer == PlayerTurn.AI;
    }
}
