using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTurnUI : MonoBehaviour
{
    public static ChooseTurnUI Instance { get; private set; }

    public event EventHandler<OnChooseTurnEventArgs> OnChooseTurn;

    public class OnChooseTurnEventArgs : EventArgs
    {
        public bool isPlayerGoFirst;
    }

    [SerializeField] private Button goFirstButton;

    [SerializeField] private Button goSecondButton;

    private void Awake()
    {
        Instance = this;

        goFirstButton.onClick.AddListener(() =>
        {
            OnChooseTurn?.Invoke(this, new OnChooseTurnEventArgs
            {
                isPlayerGoFirst = true
            });

            Hide();
        });

        goSecondButton.onClick.AddListener(() =>
        {
            OnChooseTurn?.Invoke(this, new OnChooseTurnEventArgs
            {
                isPlayerGoFirst = false
            });

            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
