using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private void Start()
    {
        FaceTheCamera();
    }

    public void FaceTheCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
