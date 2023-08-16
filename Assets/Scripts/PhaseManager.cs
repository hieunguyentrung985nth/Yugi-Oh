using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }

    private const float TIME_DELAY_BETWEEN_PHASE = 0.75f;

    private const float TIME_DELAY_BETWEEN_TURN = 2f;

    public enum Phase
    {
        Draw,
        Standby,
        Main1,
        Battle,
        Main2,
        End
    }

    public event EventHandler<OnPhaseChangeEventArgs> OnPhaseChange;

    public event EventHandler OnEndPhase;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public Phase phase;
    }

    private Phase currentPhase = Phase.Draw;

    private bool canChangePhase;

    private bool firstTurn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStartDrawPhase += GameManager_OnStartDrawPhase;
    }

    private void GameManager_OnStartDrawPhase(object sender, System.EventArgs e)
    {
        StartCoroutine(ChangePhase(Phase.Draw));
    }

    public IEnumerator ChangePhase(Phase newPhase)
    {
        TurnManager.Instance.GetCurrentTurn().ChangeState(TurnManager.Instance.GetCurrentTurn().GetGameState(Character.State.GameStateWaiting));

        if (IsBattlePhase())
        {
            BattleState.Instance.EndBattlePhase();
        }

        currentPhase = newPhase;

        if (IsBattlePhase())
        {
            BattleState.Instance.StartStep();
        }

        OnPhaseChange?.Invoke(this, new OnPhaseChangeEventArgs
        {
            phase = currentPhase
        });

        Debug.Log("Phase: " + currentPhase);

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_PHASE);

        switch (currentPhase)
        {
            case Phase.Draw:

                yield return StartCoroutine(DrawPhase());

                break;

            case Phase.Standby:

                yield return StartCoroutine(StandbyPhase());

                break;

            case Phase.Main1:

                MainPhase1();

                break;

            case Phase.Battle:

                BattlePhase();

                break;

            case Phase.Main2:

                MainPhase2();

                break;

            case Phase.End:

                yield return StartCoroutine(EndPhase());

                break;
        }
    }

    private IEnumerator DrawPhase()
    {
        yield return StartCoroutine(TurnManager.Instance.GetCurrentTurn().DrawCard());

        yield return StartCoroutine(ChangePhase(Phase.Standby));
    }

    private IEnumerator StandbyPhase()
    {
        yield return StartCoroutine(ChangePhase(Phase.Main1));
    }

    private void MainPhase1()
    {
        TurnManager.Instance.GetCurrentTurn().ChangeState(TurnManager.Instance.GetCurrentTurn().GetGameState(Character.State.GameStateNormal));
    }

    private void BattlePhase()
    {
        TurnManager.Instance.GetCurrentTurn().ChangeState(TurnManager.Instance.GetCurrentTurn().GetGameState(Character.State.GameStateBattle));
    }

    private void MainPhase2()
    {
        TurnManager.Instance.GetCurrentTurn().ChangeState(TurnManager.Instance.GetCurrentTurn().GetGameState(Character.State.GameStateNormal));
    }

    public IEnumerator EndPhase()
    {
        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_TURN / 3f);

        OnEndPhase?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_TURN / 2f);

        yield return StartCoroutine(ChangePhase(Phase.Draw));
    }

    public bool CanChangePhase()
    {
        return canChangePhase;
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }

    public bool IsMainPhase()
    {
        return currentPhase == Phase.Main1 || currentPhase == Phase.Main2;
    }

    public bool IsBattlePhase()
    {
        return currentPhase == Phase.Battle;
    }
}
