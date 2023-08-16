using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateIdle : GameState
{
    public GameStateIdle(Character character) : base(character)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExecuteState()
    {
        character.TurnOffPlayerCardInteraction();

        character.TurnOffPlayerCardPlayOnHand();

        character.TurnOffPlayerInput();

        character.TurnOffPlayerChangePhase();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
