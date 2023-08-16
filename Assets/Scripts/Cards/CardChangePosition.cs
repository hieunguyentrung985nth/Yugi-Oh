using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChangePosition : MonoBehaviour
{
    private MonsterCard monsterCard;

    private bool haveChangedThatTurn;

    private bool playedThatTurn;

    private float timeRotating = 1f;

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
        monsterCard = GetComponent<MonsterCard>();
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

    public void ResetHaveChangedThatTurn()
    {
        haveChangedThatTurn = false;
    }

    public bool HaveChangedThatTurn()
    {
        return haveChangedThatTurn;
    }

    public void SetHavePlayedThatTurn()
    {
        playedThatTurn = true;
    }

    public void ResetHavePlayedThatTurn()
    {
        playedThatTurn = false;
    }

    public bool HavePlayedThatTurn()
    {
        return playedThatTurn;
    }

    public IEnumerator ChangePosition(Character character)
    {
        if (monsterCard.GetMonsterCardData().cardPosition == CardPosition.Attack)
        {
            monsterCard.GetCardVisual().RotateToDefend();

            monsterCard.UpdateCardPosition(CardPosition.DefendFaceup);
        }

        else
        {
            monsterCard.GetCardVisual().RotateToAttack();

            monsterCard.Flip();

            monsterCard.UpdateCardPosition(CardPosition.Attack);
        }

        haveChangedThatTurn = true;

        monsterCard.TurnOffCanChangePosition();

        PositionsManager.Instance.RemoveMonsterCanChange(monsterCard);

        PositionsManager.Instance.TurnOnChangePosition();

        monsterCard.GetCardVisual().CardNormalStateOnField();

        monsterCard.GetMonsterZoneSingle().SetUpText();

        yield return new WaitForSeconds(timeRotating);

        BattleSystem.Instance.SetCharacterAction(character);

        BattleSystem.Instance.OnActionsFinishedEvent();
    }

    private void OnMouseEnter()
    {
        if (selectable &&
            Player.Instance.PlayerInputEnabled && !isHover)
        {
            monsterCard.GetCardVisual().CardActiveOnField();

            BattleSystem.Instance.TooltipCardChangePositionEvent(monsterCard);
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
            selectable = false;

            TooltipManager.Instance.TurnOffTooltip();

            StartCoroutine(ChangePosition(Player.Instance));

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
