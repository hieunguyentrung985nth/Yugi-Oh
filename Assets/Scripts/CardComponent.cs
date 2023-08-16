using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardComponent : MonoBehaviour
{
    protected bool selectable;

    protected virtual void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        TurnOffSelectable();
    }

    public void TurnOffSelectable()
    {
        selectable = false;
    }

    public void TurnOnSelectable()
    {
        selectable = true;
    }
}
