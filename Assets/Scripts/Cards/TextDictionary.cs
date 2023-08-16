using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TextDictionary : MonoBehaviour
{
    public static TextDictionary Instance { get; private set; }

    [SerializeField] private TextBoxUI addCardFromDeckToHand;

    [SerializeField] private TextBoxUI specialSummonFromGraveyard;

    private void Awake()
    {
        Instance = this;
        //if (cardsToSelect.Count != 0)
        //{
        //    res = "Chọn lá từ bộ bài để thêm vào tay.";
        //}

        //else
        //{
        //    res = "Không có lá nào trong bộ bài để thêm vào tay.";
        //}
    }

    public TextBoxUI GetTextBox(TextBoxType textBoxType)
    {
        switch (textBoxType)
        {
            case TextBoxType.AddCardFromDeckToHand:

                return addCardFromDeckToHand;

            case TextBoxType.SpecialSummonFromGraveyard:

                return specialSummonFromGraveyard;

            default:

                return null;
        }
    }
}
