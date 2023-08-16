using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextBoxUI
{
    [TextArea(3,5)]
    public string success;

    [TextArea(3,5)]
    public string failed;
}
