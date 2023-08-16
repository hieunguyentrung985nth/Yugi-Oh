using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAttackOnField : MonoBehaviour
{
    private MonsterCard monsterCard;

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

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        TurnOffSelectable();
    }

    private void Awake()
    {
        monsterCard = GetComponent<MonsterCard>();
    }

    private void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (selectable && Player.Instance.PlayerInputEnabled && !isHover)
        {
            monsterCard.GetCardVisual().CardActiveOnField();

            BattleSystem.Instance.TooltipCardAttackEvent(monsterCard);
        }
    }

    private void OnMouseExit()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            monsterCard.GetCardVisual().CardNormalStateOnField();

            TooltipManager.Instance.TurnOffTooltip();
        }

        isHover = false;
    }

    private void OnMouseDown()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            TooltipManager.Instance.TurnOffTooltip();

            CardInfoUI.Instance.ShowCardImmediately(monsterCard);

            selectable = false;

            BattleState.Instance.SetCurrentMonsterAttack(monsterCard);

            isHover = false;
        }
    }

    private void OnMouseOver()
    {
        if (selectable && Player.Instance.PlayerInputEnabled && !isHover)
        {
            OnMouseEnter();

            isHover = true;
        }
    }
}
