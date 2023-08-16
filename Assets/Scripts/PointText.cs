using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
    public static event EventHandler<OnDecreasePointEventArgs> OnDecreasePoint;

    public class OnDecreasePointEventArgs : EventArgs
    {
        public int pointDecrease;

        public Character owner;
    }

    [SerializeField] private Color32 decreaseColor;

    [SerializeField] private Color32 increaseColor;

    [SerializeField] private TextMeshProUGUI pointText;

    private float timeShowing = 1f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Hide();
    }

    public void SpawnAtPosition(Vector3 targetPos)
    {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(targetPos);

        transform.position = spawnPos;
    }

    public IEnumerator DecreasePoint(int value, Vector3 targetPos, bool selfDecrease, Character owner)
    {
        if (value != 0)
        {
            pointText.text = "-" + value.ToString();
        }

        else
        {
            pointText.text = "";
        }

        ChangeToDecreaseColor();

        SpawnAtPosition(targetPos);

        Show(value, owner, selfDecrease);

        yield return StartCoroutine(DelayBeforeHide());
    }

    public IEnumerator IncreasePoint(int value, Vector3 targetPos, Character owner)
    {
        pointText.text = "+" + value.ToString();

        ChangeToIncreaseColor();

        SpawnAtPosition(targetPos);

        Show(value, owner);

        yield return StartCoroutine(DelayBeforeHide());
    }

    private void ChangeToDecreaseColor()
    {
        pointText.color = decreaseColor;
    }

    private void ChangeToIncreaseColor()
    {
        pointText.color = increaseColor;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(int value, Character owner, bool selfDecrease = false)
    {
        if (!selfDecrease)
        {
            OnDecreasePoint?.Invoke(this, new OnDecreasePointEventArgs
            {
                pointDecrease = value,

                owner = owner
            });
        }

        gameObject.SetActive(true);
    }

    private IEnumerator DelayBeforeHide()
    {
        yield return new WaitForSeconds(timeShowing);

        Hide();
    }
}
