using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateWaiting : GameState
{
    public GameStateWaiting(Character character) : base(character)
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
