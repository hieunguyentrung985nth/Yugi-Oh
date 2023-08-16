using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTribute : MonoBehaviour
{
    private MonsterCard monsterCard;

    private bool selectable;

    private void Awake()
    {
        monsterCard = GetComponent<MonsterCard>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void TurnOnSelectable()
    {
        selectable = true;
    }

    public void TurnOffSelectable()
    {
        selectable = false;
    }

    private void OnMouseEnter()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            monsterCard.GetCardVisual().CardActiveOnField();
        }
    }

    private void OnMouseExit()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            monsterCard.GetCardVisual().CardNormalStateOnField();
        }
    }

    private void OnMouseDown()
    {
        if (selectable && Player.Instance.PlayerInputEnabled)
        {
            selectable = false;

            Debug.Log("Tribute!");

            monsterCard.GetCardVisual().CardNormalStateOnField();

            StartCoroutine(TributeManager.Instance.MarkAsTribute(monsterCard));
        }
    }
}
