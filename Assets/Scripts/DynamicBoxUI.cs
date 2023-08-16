using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private Button yesButton;

    [SerializeField] private Button noButton;

    [SerializeField] private Button okButton;

    private SpellCardEffect currentEffect;

    private void Awake()
    {
        yesButton.onClick.AddListener(() =>
        {
            BattleSystem.Instance.OnCarryActionEvent();

            Hide();
        });

        noButton.onClick.AddListener(() =>
        {
            BoxUI.Instacne.CancelAction();

            Hide();
        });

        okButton.onClick.AddListener(() =>
        {
            Hide();

            currentEffect.ConfirmAction();
        });
    }

    private void Start()
    {
        Hide();
    }

    public void SetText(string value, BoxType boxType, SpellCardEffect effect = null)
    {
        if (effect != null)
        {
            currentEffect = effect;
        }

        contentText.text = value;

        switch (boxType)
        {
            case BoxType.YesNoType:

                ShowYesNoButton();

                break;
            case BoxType.OkType:

                ShowOkButton();

                break;

            default:

                ShowOkButton();

                break;
        }

        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void ShowYesNoButton()
    {
        yesButton.gameObject.SetActive(true);

        noButton.gameObject.SetActive(true);

        okButton.gameObject.SetActive(false);
    }

    private void ShowOkButton()
    {
        yesButton.gameObject.SetActive(false);

        noButton.gameObject.SetActive(false);

        okButton.gameObject.SetActive(true);
    }
}
