using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private Transform canvasTransform;

    [SerializeField] private Tooltip tooltip;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BattleSystem.Instance.OnCardOnHandHoverTooltip += BattleSystem_OnCardHoverTooltip;

        BattleSystem.Instance.OnCardChangePositionHoverTooltip += BattleSystem_OnCardChangePositionHoverTooltip;

        BattleSystem.Instance.OnCardAttackHoverTooltip += BattleSystem_OnCardAttackHoverTooltip;

        BattleSystem.Instance.OnCardActiveHoverTooltip += BattleSystem_OnCardActiveHoverTooltip;
    }

    private void BattleSystem_OnCardActiveHoverTooltip(object sender, BattleSystem.OnCardHoverTooltipEventArgs e)
    {
        tooltip.Active();
    }

    private void BattleSystem_OnCardAttackHoverTooltip(object sender, BattleSystem.OnCardHoverTooltipEventArgs e)
    {
        tooltip.Attack();
    }

    private void BattleSystem_OnCardChangePositionHoverTooltip(object sender, BattleSystem.OnCardHoverTooltipEventArgs e)
    {
        tooltip.ChangePosition(e.card);
    }

    private void BattleSystem_OnCardHoverTooltip(object sender, BattleSystem.OnCardHoverTooltipEventArgs e)
    {
        tooltip.CardOnHand(e.card);
    }

    public void TurnOffTooltip()
    {
        tooltip.Hide();
    }
}
