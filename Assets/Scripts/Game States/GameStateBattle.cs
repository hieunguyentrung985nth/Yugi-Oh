using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameStateBattle : GameState
{
    public GameStateBattle(Character character) : base(character)
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

        character.TurnOnPlayerInput();

        character.TurnOnPlayerChangePhase();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
