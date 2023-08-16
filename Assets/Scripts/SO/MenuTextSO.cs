using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MenuTextSO : ScriptableObject
{
    [TextArea(5,10)]
    public List<string> texts;
}
