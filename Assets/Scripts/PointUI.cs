using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUI : MonoBehaviour
{
    public static PointUI Instance { get; private set; }

    [SerializeField] private PointSingle aIPoint;

    [SerializeField] private PointSingle playerPoint;

    [SerializeField] private Color32 defaultColorPointText;

    [SerializeField] private Color32 dangerColorPointText;

    [SerializeField] private PointText pointTextPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public IEnumerator DecreasePoint(int value, Vector3 targetPos, Character character)
    {
        if (character is Player)
        {
            yield return StartCoroutine(playerPoint.DecreasePoint(value, targetPos));
        }

        else
        {
            yield return StartCoroutine(aIPoint.DecreasePoint(value, targetPos));
        }
    }

    public IEnumerator IncreasePoint(int value, Vector3 targetPos, Character character)
    {
        if (character is Player)
        {
            yield return StartCoroutine(playerPoint.IncreasePoint(value, targetPos));
        }

        else
        {
            yield return StartCoroutine(aIPoint.IncreasePoint(value, targetPos));
        }
    }

    public Color32 GetDefaultPointColorText()
    {
        return defaultColorPointText;
    }

    public Color32 GetDangerPointColorText()
    {
        return dangerColorPointText;
    }

    public PointText GetPointTextPrefab()
    {
        return pointTextPrefab;
    }
}
