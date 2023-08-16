using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUI : MonoBehaviour
{
    public static PhaseUI Instance { get; private set; }

    [SerializeField] private List<PhaseButton> phaseButtonList;

    [SerializeField] private Color32 greenBackgroundColor;

    [SerializeField] private Color32 redBackgroundColor;

    [SerializeField] private Color32 greenTextColor;

    [SerializeField] private Color32 redTextColor;

    [SerializeField] private Color32 hightlightColor;

    private PhaseButton previousHighlightButton;

    private bool firstTurn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChooseTurnUI.Instance.OnChooseTurn += ChooseTurnUI_OnChooseTurn;

        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;

        Character.OnCanChangePhase += Character_OnCanChangePhase;

        Character.OnCantChangePhase += Character_OnCantChangePhase;

        PhaseManager.Instance.OnEndPhase += PhaseManager_OnChangeTurn;

        BattleSystem.Instance.OnActionsFinished += BattleSystem_OnActionsFinished;
    }

    private void OnDestroy()
    {
        Character.OnCanChangePhase -= Character_OnCanChangePhase;

        Character.OnCantChangePhase -= Character_OnCantChangePhase;
    }

    private void BattleSystem_OnActionsFinished(object sender, Character.OnCharacterTriggerEventEventArgs e)
    {
        if (TurnManager.Instance.IsPlayerTurn() && BattleState.Instance.CanChangeToBattleState() && !firstTurn
            && PhaseManager.Instance.GetCurrentPhase() == PhaseManager.Phase.Main1)
        {
            GetPhaseButton(PhaseManager.Phase.Battle).TurnOnButton();
        }

        else
        {
            GetPhaseButton(PhaseManager.Phase.Battle).TurnOffButton();
        }
    }

    private void PhaseManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            ChangeColorToGreen();
        }

        else
        {
            ChangeColorToRed();
        }

        TurnOffButton();

        firstTurn = false;
    }

    private void Character_OnCanChangePhase(object sender, System.EventArgs e)
    {
        TurnOnPossibleButtons();

        if (TurnManager.Instance.IsPlayerTurn() && BattleState.Instance.CanChangeToBattleState() && !firstTurn && PhaseManager.Instance.GetCurrentPhase() == PhaseManager.Phase.Main1)
        {
            GetPhaseButton(PhaseManager.Phase.Battle).TurnOnButton();
        }

        else
        {
            GetPhaseButton(PhaseManager.Phase.Battle).TurnOffButton();
        }

        if (firstTurn)
        {
            TurnOffBattlePhaseFirstTurn();
        }       
    }

    private void Character_OnCantChangePhase(object sender, System.EventArgs e)
    {
        TurnOffPossibleButtons();
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        HandlePhaseChangeButton(e.phase);
    }

    private void ChooseTurnUI_OnChooseTurn(object sender, ChooseTurnUI.OnChooseTurnEventArgs e)
    {
        if (e.isPlayerGoFirst)
        {
            ChangeColorToGreen();
        }

        else
        {
            ChangeColorToRed();
        }

        TurnOffButton();

        firstTurn = true;
    }

    private void HandlePhaseChangeButton(PhaseManager.Phase phase)
    {
        TurnOffHighlightButton();

        previousHighlightButton = GetPhaseButton(phase);

        previousHighlightButton.HighlightButton(hightlightColor);   
    }

    private void ChangeColorToGreen()
    {
        foreach (PhaseButton button in phaseButtonList)
        {
            button.ChangeColor(greenBackgroundColor, greenTextColor);
        }
    }

    private void ChangeColorToRed()
    {
        foreach (PhaseButton button in phaseButtonList)
        {
            button.ChangeColor(redBackgroundColor, redTextColor);
        }
    }

    private void TurnOffButton()
    {
        foreach (PhaseButton button in phaseButtonList)
        {
            button.TurnOffButton();
        }
    }

    private void TurnOffBattlePhaseFirstTurn()
    {
        GetPhaseButton(ConvertPhaseButtonTypeToPhase(PhaseButton.PhaseButtonType.Battle)).TurnOffButton();
    }

    private void TurnOffHighlightButton()
    {
        if (previousHighlightButton != null)
        {
            previousHighlightButton.ChangeToNormalState();
        }
    }

    private PhaseButton GetPhaseButton(PhaseManager.Phase phase)
    {
        foreach (PhaseButton button in phaseButtonList)
        {
            if (ConvertPhaseButtonTypeToPhase(button.GetPhaseButtonType()) == phase)
            {
                return button;
            }
        }

        return null;
    }

    private PhaseManager.Phase ConvertPhaseButtonTypeToPhase(PhaseButton.PhaseButtonType phaseButtonType)
    {
        switch (phaseButtonType)
        {
            case PhaseButton.PhaseButtonType.Draw:

                return PhaseManager.Phase.Draw;

            case PhaseButton.PhaseButtonType.Standby:

                return PhaseManager.Phase.Standby;

            case PhaseButton.PhaseButtonType.Main1:

                return PhaseManager.Phase.Main1;

            case PhaseButton.PhaseButtonType.Battle:

                return PhaseManager.Phase.Battle;

            case PhaseButton.PhaseButtonType.Main2:

                return PhaseManager.Phase.Main2;

            case PhaseButton.PhaseButtonType.End:

                return PhaseManager.Phase.End;

            default:
                return default;
        }
    }

    public Color32 GetHighlightColor()
    {
        return hightlightColor;
    }

    private void TurnOnPossibleButtons()
    {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            switch (PhaseManager.Instance.GetCurrentPhase())
            {
                case PhaseManager.Phase.Draw:
                    break;
                case PhaseManager.Phase.Standby:
                    break;

                case PhaseManager.Phase.Main1:

                    GetPhaseButton(PhaseManager.Phase.Battle).TurnOnButton();

                    GetPhaseButton(PhaseManager.Phase.End).TurnOnButton();

                    break;

                case PhaseManager.Phase.Battle:

                    GetPhaseButton(PhaseManager.Phase.Battle).TurnOffButton();

                    GetPhaseButton(PhaseManager.Phase.Main2).TurnOnButton();

                    GetPhaseButton(PhaseManager.Phase.End).TurnOnButton();

                    break;

                case PhaseManager.Phase.Main2:

                    GetPhaseButton(PhaseManager.Phase.Battle).TurnOffButton();

                    GetPhaseButton(PhaseManager.Phase.End).TurnOnButton();

                    break;

                case PhaseManager.Phase.End:
                    break;

                default:
                    break;
            }
        }
    }

    private void TurnOffPossibleButtons()
    {
        GetPhaseButton(PhaseManager.Phase.Battle).TurnOffButton();

        GetPhaseButton(PhaseManager.Phase.Main2).TurnOffButton();

        GetPhaseButton(PhaseManager.Phase.End).TurnOffButton();
    }
}
