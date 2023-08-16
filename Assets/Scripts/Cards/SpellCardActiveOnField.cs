using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardActiveOnField : MonoBehaviour
{
    private SpellCard spellCard;

    private bool selectable;

    private bool isHover;

    public void TurnOffSelectable()
    {
        selectable = false;
    }

    public void TurnOnSelectable()
    {
        selectable = true;
    }

    private void Awake()
    {
        spellCard = GetComponent<SpellCard>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        TurnOffSelectable();
    }

    private void Update()
    {

    }

    private void OnMouseEnter()
    {
        if (selectable && Player.Instance.PlayerCardInteractionEnabled && !isHover)
        {
            spellCard.GetCardVisual().CardActiveOnField();

            BattleSystem.Instance.TooltipCardActiveEvent(spellCard);
        }
    }

    private void OnMouseExit()
    {
        if (selectable && Player.Instance.PlayerCardInteractionEnabled)
        {
            spellCard.GetCardVisual().CardNormalStateOnField();

            TooltipManager.Instance.TurnOffTooltip();
        }

        isHover = false;
    }

    private void OnMouseDown()
    {
        if (selectable && Player.Instance.PlayerCardInteractionEnabled)
        {
            TooltipManager.Instance.TurnOffTooltip();

            selectable = false;

            StartCoroutine(Player.Instance.ActiveSpellCardOnField(spellCard));

            Player.Instance.ChangeState(Player.Instance.GetGameState(Character.State.GameStateWaiting));

            BattleSystem.Instance.SetCharacterAction(Player.Instance);

            isHover = false;
        }
    }

    private void OnMouseOver()
    {
        if (selectable && Player.Instance.PlayerCardInteractionEnabled && !isHover)
        {
            OnMouseEnter();

            isHover = true;
        }
    }
}
