using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameStateTribute : GameState
{
    public GameStateTribute(Character character) : base(character)
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

        character.TurnOffPlayerChangePhase();

        TributeManager.Instance.TurnOnSelectableForTribute();

        PositionsManager.Instance.TurnOffChangePosition();

        // turn off effect
    }

    public override void ExitState()
    {
        base.ExitState();

        TributeManager.Instance.TurnOffSelectableForTribute();

        PositionsManager.Instance.TurnOnChangePosition();
    }
}
