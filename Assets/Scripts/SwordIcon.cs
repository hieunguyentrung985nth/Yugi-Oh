using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordIcon : MonoBehaviour
{
    [SerializeField] private SwordIconVisual swordIconVisual;

    private void Start()
    {
        
    }

    public SwordIconVisual GetSwordIconVisual()
    {
        return swordIconVisual;
    }

    public void ShowSwordIcon()
    {
        swordIconVisual.Show();
    }

    public void HideSwordIcon()
    {
        swordIconVisual.Hide();

        GetSwordIconVisual().ResetSword();
    }

    public void EnableRotateByMouse()
    {
        swordIconVisual.EnableRotateByMouse();
    }
}
