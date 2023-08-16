using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhaseButton : MonoBehaviour
{
    [Serializable]
    public enum PhaseButtonType
    {
        Draw,
        Standby,
        Main1,
        Battle,
        Main2,
        End
    }

    [SerializeField] private Image backgroundImage;

    [SerializeField] private TextMeshProUGUI textButton;

    [SerializeField] private PhaseButtonType phaseButtonType;

    private Button button;

    private Color32 defaultBackgroundColor;

    private Color32 defaultTextColor;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            StartCoroutine(ButtonFunction());
        });
    }

    private IEnumerator ButtonFunction()
    {
        switch (phaseButtonType)
        {          
            case PhaseButtonType.Battle:

                yield return StartCoroutine(PhaseManager.Instance.ChangePhase(PhaseManager.Phase.Battle));

                break;

            case PhaseButtonType.Main2:

                yield return StartCoroutine(PhaseManager.Instance.ChangePhase(PhaseManager.Phase.Main2));

                break;

            case PhaseButtonType.End:

                yield return StartCoroutine(PhaseManager.Instance.ChangePhase(PhaseManager.Phase.End));

                break;

            default:
                break;
        }
    }

    public void ChangeToNormalState()
    {
        backgroundImage.color = defaultBackgroundColor;

        textButton.color = defaultTextColor;
    }

    public void ChangeColor(Color32 backgroundColor, Color32 textColor)
    {
        backgroundImage.color = backgroundColor;

        textButton.color = textColor;

        defaultBackgroundColor = backgroundImage.color;

        defaultTextColor = textButton.color;
    }

    public void HighlightButton(Color32 highlightColor)
    {
        backgroundImage.color = highlightColor;
    }

    public void TurnOffButton()
    {
        button.enabled = false;
    }

    public void TurnOnButton()
    {
        button.enabled = true;
    }

    public PhaseButtonType GetPhaseButtonType()
    {
        return phaseButtonType;
    }
}
