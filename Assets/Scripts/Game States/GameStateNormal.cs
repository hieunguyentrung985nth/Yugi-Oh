using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateNormal : GameState
{
    public GameStateNormal(Character character) : base(character)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExecuteState()
    {
        character.TurnOnPlayerCardInteraction();

        character.TurnOnPlayerCardPlayOnHand();

        character.TurnOnPlayerInput();

        character.TurnOnPlayerChangePhase();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
