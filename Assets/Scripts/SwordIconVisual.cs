using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordIconVisual : MonoBehaviour
{
    public static event EventHandler OnSwordReachTarget;

    private float rotationSpeed = 0.25f;

    private float moveSpeed = 0.25f;

    private Vector3 initialRotation;

    private Vector3 initialPosition;

    private bool canRotateByMouse;

    private float timeHideDelay = 0.5f;

    private void Start()
    {
        initialRotation = transform.localEulerAngles;

        initialPosition = transform.localPosition;

        Hide();
    }

    private void Update()
    {
        RotateByMouse();
    }

    public void EnableRotateByMouse()
    {
        canRotateByMouse = true;
    }

    public void DisableRotateByMouse()
    {
        canRotateByMouse = false;
    }

    private void RotateByMouse()
    {
        if (canRotateByMouse)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 dir = (mousePosition - transform.position).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.localEulerAngles = new Vector3(0, 0, angle + 180);
        }
    }

    public void RotateToPoint(Vector3 target)
    {
        DisableRotateByMouse();

        Vector3 dir = (target - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        LeanTween.rotateZ(gameObject, angle + 180, rotationSpeed)
            .setOnComplete(() => {
                MoveToPoint(target);
            });
    }

    public void MoveToPoint(Vector3 target)
    {
        Vector3 startPos = transform.position;

        LeanTween.move(gameObject, target, moveSpeed)
            .setFrom(startPos)
            .setOnComplete(() =>
            {
                StartCoroutine(OnSwordReachTargetEvent());
            });
    }

    private IEnumerator OnSwordReachTargetEvent()
    {
        yield return new WaitForSeconds(timeHideDelay);

        Hide();

        OnSwordReachTarget?.Invoke(this, EventArgs.Empty);
    }

    public void ResetSword()
    {
        transform.localPosition = initialPosition;

        transform.localEulerAngles = initialRotation;

        DisableRotateByMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
