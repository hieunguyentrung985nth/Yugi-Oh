using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSingle : MonoBehaviour
{
    [SerializeField] private Character owner;

    [SerializeField] private TextMeshProUGUI pointText;

    public static int dangerPoints { get => 2500; private set { } }

    private void Start()
    {
        InitializePoint();
    }

    public IEnumerator IncreasePoint(int value, Vector3 targetPos)
    {      
        yield return StartCoroutine(PointUI.Instance.GetPointTextPrefab().IncreasePoint(value, targetPos, owner));

        owner.IncreaseLifePoints(value);

        if (int.Parse(pointText.ToString()) <= dangerPoints)
        {
            ChangeToDangerColor();
        }

        else
        {
            ChangeToDefaultColor();
        }

        pointText.text = owner.GetLifePoints().ToString();
    }

    public IEnumerator DecreasePoint(int value, Vector3 targetPos, bool selfDecrease = false)
    {
        yield return StartCoroutine(PointUI.Instance.GetPointTextPrefab().DecreasePoint(value, targetPos, selfDecrease, owner));

        owner.DecreaseLifePoints(value);

        if (owner.GetLifePoints() <= dangerPoints)
        {
            ChangeToDangerColor();
        }

        else
        {
            ChangeToDefaultColor();
        }

        pointText.text = owner.GetLifePoints().ToString();
    }

    private void ChangeToDefaultColor()
    {
        pointText.color = PointUI.Instance.GetDefaultPointColorText();
    }

    private void ChangeToDangerColor()
    {
        pointText.color = PointUI.Instance.GetDangerPointColorText();
    }

    public void InitializePoint()
    {
        pointText.text = owner.GetLifePoints().ToString();

        ChangeToDefaultColor();
    }
}
