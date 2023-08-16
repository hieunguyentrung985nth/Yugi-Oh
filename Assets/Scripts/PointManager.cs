using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator DecreasePoint(int value, Vector3 targetPos, Character character)
    {
        yield return StartCoroutine(PointUI.Instance.DecreasePoint(value, targetPos, character));
    }

    public IEnumerator IncreasePoint(int value, Vector3 targetPos, Character character)
    {
        yield return StartCoroutine(PointUI.Instance.IncreasePoint(value, targetPos, character));
    }
}
