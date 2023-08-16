using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIVoiceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI voiceText;

    [SerializeField] private float timeText;

    [SerializeField] private float timeDelay;

    public IEnumerator SetText(List<string> voiceLines)
    {
        int currentIndex = 0;

        string text = "";

        for (int i = 0; i < voiceLines.Count; i++)
        {
            currentIndex = 0;

            while (currentIndex < voiceLines[i].Length)
            {
                currentIndex += 1;

                text = voiceLines[i].Substring(0, currentIndex);

                text += "<color=#00000000>" + voiceLines[i].Substring(currentIndex) + "</color>";

                voiceText.text = text;

                yield return new WaitForSecondsRealtime(timeText);
            }

            yield return new WaitForSecondsRealtime(timeDelay);
        }

        yield return new WaitForSecondsRealtime(timeDelay);
    }
}
