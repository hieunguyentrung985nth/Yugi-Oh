using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class BoxUI : MonoBehaviour
{
    public static BoxUI Instacne { get; private set; }

    public event EventHandler OnBoxShow;

    public event EventHandler OnBoxCancel;

    [SerializeField] private Color32 colorSpecialAction;

    [SerializeField] private Color32 colorTributeNumber;

    [SerializeField] private Color32 colorCardName;

    [SerializeField] private DynamicBoxUI dynamicBox;

    [SerializeField] private SelectBoxUI selectBox;

    [SerializeField][TextArea(3, 5)] private string tributeText;

    private List<SpellCardEffect> effectsList;

    private SpellCardEffect currentEffect;

    private void Awake()
    {
        Instacne = this;

        effectsList = new List<SpellCardEffect>();
    }

    private void Start()
    {
        TributeManager.Instance.OnTributeStart += TributeManager_OnTributeStart;

        EffectsManager.Instance.OnEffectStart += EffectsManager_OnEffectStart;
    }

    private void EffectsManager_OnEffectStart(object sender, EffectsManager.OnEffectStartEventArgs e)
    {
        effectsList = e.effects;
    }

    private void TributeManager_OnTributeStart(object sender, TributeManager.OnTributeStartEventArgs e)
    {
        StartTribute(e);
    }

    private void StartTribute(TributeManager.OnTributeStartEventArgs e)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>
        {
            {
                "{colorSpecialAction}", "<color=#" + ColorUtility.ToHtmlStringRGBA(colorSpecialAction)+">"
            },
            {
                "{colorTributeNumber}", "<color=#"+ ColorUtility.ToHtmlStringRGBA(colorTributeNumber)+">"
            },
            {
                "{numberForTribute}", e.numberForTribute.ToString()
            }
        };

        string res = ReplaceStringVariable(dictionary, tributeText);

        dynamicBox.SetText(res, BoxType.YesNoType);

        OnBoxShow?.Invoke(this, EventArgs.Empty);

        UndoAction.Instance.SetUndoState(UndoAction.UndoActionState.Tribute);
    }

    public void StartSelectCard(SpellCardEffect effect, string textToShow)
    {
        currentEffect = effect;

        dynamicBox.SetText(textToShow, BoxType.OkType, effect);

        OnBoxShow?.Invoke(this, EventArgs.Empty);
    }

    private string ReplaceStringVariable(Dictionary<string, string> dictionary, string textToReplace)
    {
        string res = textToReplace;

        foreach (string var in dictionary.Keys)
        {
            res = res.Replace(var, dictionary[var]);
        }

        return res;
    }

    public void CancelAction()
    {
        OnBoxCancel?.Invoke(this, EventArgs.Empty);
    }

    public void ShowBoxAgain()
    {
        dynamicBox.Show();
    }

    public DynamicBoxUI GetDynamicBox()
    {
        return dynamicBox;
    }

    public SelectBoxUI GetSelectBox()
    {
        return selectBox;
    }

    public SpellCardEffect GetCurrentEffect()
    {
        return currentEffect;
    }
}
