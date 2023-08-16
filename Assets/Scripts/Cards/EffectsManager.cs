using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    public event EventHandler<OnEffectStartEventArgs> OnEffectStart;

    public class OnEffectStartEventArgs : EventArgs
    {
        public List<SpellCardEffect> effects;
    }

    private List<SpellTrapDefault> spellTrapFaceupList;

    private List<SpellTrapDefault> spellSpeed1Facedown;

    private Queue<SpellTrapDefault> monsterTriggerEffectsList;

    private Stack<SpellTrapDefault> effectsStack;

    private SpellTrapDefault currentSpellEffect;

    private void Awake()
    {
        Instance = this;

        spellTrapFaceupList = new List<SpellTrapDefault>();

        spellSpeed1Facedown = new List<SpellTrapDefault>();

        monsterTriggerEffectsList = new Queue<SpellTrapDefault>();

        effectsStack = new Stack<SpellTrapDefault>();
    }

    private void Start()
    {
        BattleSystem.Instance.OnActionsFinished += BattleSystem_OnActionsFinished;

        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        CheckSpellSpeed1Active();
    }

    private void BattleSystem_OnActionsFinished(object sender, Character.OnCharacterTriggerEventEventArgs e)
    {
        CheckSpellSpeed1Active();
    }

    public void CheckSpellSpeed1Active()
    {
        foreach (SpellTrapDefault card in spellSpeed1Facedown)
        {
            SpellCard spellCard = card.GetSpellTrapCard() as SpellCard;

            if (spellCard.GetOwner() == TurnManager.Instance.GetCurrentTurn() && PhaseManager.Instance.IsMainPhase() && spellCard.CanActiveCard())
            {
                spellCard.GetComponent<SpellCardActiveOnField>().TurnOnSelectable();
            }

            else
            {
                spellCard.GetComponent<SpellCardActiveOnField>().TurnOffSelectable();
            }
        }
    }

    public void GetAllSpellEffects()
    {

    }

    public void AddNormalSpellEffect(SpellTrapDefault effect)
    {
        spellTrapFaceupList.Add(effect);
    }

    public void AddSpellSpeed1FacedownEffect(SpellTrapDefault effect)
    {
        spellSpeed1Facedown.Add(effect);
    }

    public void RemoveSpellSpeed1FacedownEffect(SpellTrapDefault effect)
    {
        spellSpeed1Facedown.Remove(effect);
    }

    public void AddMonsterTriggerEffect(SpellTrapDefault effect)
    {
        monsterTriggerEffectsList.Enqueue(effect);
    }

    public IEnumerator ExecuteTriggerMonsterEffects()
    {
        if (monsterTriggerEffectsList.Count > 0)
        {
            SpellTrapDefault spellTrapDefault = monsterTriggerEffectsList.Dequeue();

            currentSpellEffect = spellTrapDefault;

            yield return StartCoroutine(currentSpellEffect.GetCard().GetCardVisual().ActiveEffect());

            OnEffectStart?.Invoke(this, new OnEffectStartEventArgs
            {
                effects = currentSpellEffect.GetResolveCardEffects()
            });

            yield return StartCoroutine(spellTrapDefault.Resolve());

            ClearCurrentEffect();
        }

        BattleSystem.Instance.OnActionsFinishedEvent();
    }

    public IEnumerator ExecuteEffects()
    {
        if (spellTrapFaceupList.Count > 0)
        {
            currentSpellEffect = spellTrapFaceupList[0];

            yield return StartCoroutine(currentSpellEffect.GetCard().GetCardVisual().ActiveEffect());

            yield return StartCoroutine(spellTrapFaceupList[0].Resolve());

            spellTrapFaceupList.Remove(spellTrapFaceupList[0]);

            ClearCurrentEffect();
        }

        BattleSystem.Instance.OnActionsFinishedEvent();
    }

    public IEnumerator FlipFacedownSpellSpeed1ToActivate(SpellTrapDefault effect)
    {
        AddStackEffect(effect);

        yield return StartCoroutine(effect.GetSpellTrapCard().GetCardVisual().FlipSpellTrap());

        yield return StartCoroutine(ExecuteStackEffects());
    }

    public IEnumerator ExecuteStackEffects()
    {
        while (effectsStack.Count > 0)
        {
            currentSpellEffect = effectsStack.Pop();

            yield return StartCoroutine(currentSpellEffect.GetCard().GetCardVisual().ActiveEffect());

            yield return StartCoroutine(currentSpellEffect.Resolve());

            currentSpellEffect = null;

            yield return null;
        }

        while (currentSpellEffect != null)
        {
            yield return null;
        }

        BattleSystem.Instance.CheckGameOverEvent();

        BattleSystem.Instance.OnActionsFinishedEvent();
    }

    public void AddStackEffect(SpellTrapDefault effect)
    {
        effectsStack.Push(effect);
    }

    public void RemoveStackEffect()
    {
        effectsStack.Pop();
    }

    public void ClearStackEffect()
    {
        effectsStack.Clear();
    }

    public SpellTrapDefault GetCurrentEffect()
    {
        return currentSpellEffect;
    }

    public void ClearCurrentEffect()
    {
        currentSpellEffect = null;
    }

    public void SetCurrentEffect(SpellTrapDefault spellTrapDefault)
    {
        currentSpellEffect = spellTrapDefault;
    }
}
