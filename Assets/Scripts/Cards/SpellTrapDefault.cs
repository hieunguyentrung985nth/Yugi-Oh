using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public abstract class SpellTrapDefault : MonoBehaviour, IEffect
{
    [SerializeField] protected List<SpellCardEffectWithState> resolveEffects;

    [SerializeField] protected List<SpellCardEffect> activationEffects;

    [SerializeField] protected List<ConditionDefault> conditionEffects;

    protected CardSO cardSO;

    [SerializeField] protected EquipStateEffect equipStateEffect;

    protected Card card;

    protected Character owner;

    protected List<SpellCardEffect> resolveEffectsList;

    protected List<SpellCardEffect> activationEffectsList;

    protected List<ConditionDefault> conditionEffectsList;

    private void Awake()
    {
    }

    private void Start()
    {
        
    }

    public void SetUp(Card card, Character owner)
    {
        this.card = card;

        this.owner = owner;

        this.cardSO = card.GetCardSO();
    }

    public void Initialized()
    {
        resolveEffectsList = new List<SpellCardEffect>();

        activationEffectsList = new List<SpellCardEffect>();

        conditionEffectsList = new List<ConditionDefault>();

        foreach (SpellCardEffect effect in GetEffects())
        {
            resolveEffectsList.Add(Instantiate(effect, transform));
        }

        foreach (SpellCardEffect effect in activationEffects)
        {
            activationEffectsList.Add(Instantiate(effect, transform));
        }

        foreach (ConditionDefault condition in conditionEffects)
        {
            conditionEffectsList.Add(Instantiate(condition, transform));
        }
    }

    private bool GetStateEffect(SpellCardEffect effect)
    {
        foreach (SpellCardEffectWithState item in resolveEffects)
        {
            if (item.effect == effect)
            {
                return item.canSkip;
            }
        }

        return default;
    }

    public List<SpellCardEffect> GetEffects()
    {
        List<SpellCardEffect> spellCardEffects = new List<SpellCardEffect>();

        foreach (SpellCardEffectWithState item in resolveEffects)
        {
            spellCardEffects.Add(item.effect);
        }

        return spellCardEffects;
    }

    public SpellCardEffect FindResolveCardEffect(SpellCardEffect effect)
    {
        foreach (SpellCardEffect child in resolveEffectsList)
        {
            if (child == effect)
            {
                return child;
            }
        }

        return null;
    }

    public SpellCardEffect FindActivationCardEffect(SpellCardEffect effect)
    {
        foreach (SpellCardEffect child in activationEffectsList)
        {
            if (child == effect)
            {
                return child;
            }
        }

        return null;
    }

    public ConditionDefault FindConditionCardEffect(ConditionDefault effect)
    {
        foreach (ConditionDefault child in conditionEffectsList)
        {
            if (child == effect)
            {
                return child;
            }
        }

        return null;
    }

    public virtual void InitializeResolveEffect(SpellCardEffect effect = null, params object[] values)
    {
        SpellCardEffect spellCardEffect = FindResolveCardEffect(effect);

        if (spellCardEffect != null)
        {
            spellCardEffect.SetUp(values);
        }
    }

    public virtual void InitializeActivationEffect(SpellCardEffect effect = null, params object[] values)
    {
        SpellCardEffect spellCardEffect = FindActivationCardEffect(effect);

        if (spellCardEffect != null)
        {
            spellCardEffect.SetUp(values);
        }
    }

    public virtual void InitializeConditionEffect(ConditionDefault effect = null, params object[] values)
    {
        ConditionDefault conditionEffect = FindConditionCardEffect(effect);

        if (conditionEffect != null)
        {
            conditionEffect.SetUp(values);
        }
    }

    public virtual void Prepare()
    {

    }

    public List<SpellCardEffect> GetResolveCardEffects()
    {
        return resolveEffectsList;
    }

    public List<SpellCardEffect> GetActivationCardEffects()
    {
        return activationEffectsList;
    }

    public List<ConditionDefault> GetConditionCardEffects()
    {
        return conditionEffectsList;
    }

    public SpellTrapCard GetSpellTrapCard()
    {
        return card as SpellTrapCard;
    }

    public Card GetCard()
    {
        return card;
    }

    public virtual void ClearEffect()
    {
        
    }

    public List<Character> TransferToCharacterType(PointToCharacter pointToCharacter)
    {
        List<Character> list = new List<Character>();

        switch (pointToCharacter)
        {
            case PointToCharacter.Player:

                list.Add(Player.Instance);

                break;
            case PointToCharacter.AI:

                list.Add(AI.Instance);

                break;
            case PointToCharacter.Both:

                list.Add(Player.Instance);

                list.Add(AI.Instance);

                break;
        }

        return list;
    }

    public virtual bool ConditionsForSpell()
    {
        switch (cardSO.spellSpeedType)
        {
            case SpellSpeedType.SpellSpeed1:

                foreach (ConditionDefault condition in conditionEffectsList)
                {
                    if (!condition.Condtions())
                    {
                        return false;
                    }
                }

                //foreach (SpellCardEffect effect in activationEffectsList)
                //{
                //    if (!effect.ConditionsToActive())
                //    {
                //        return false;
                //    }
                //}

                if (PhaseManager.Instance.IsMainPhase()
                    && TurnManager.Instance.GetCurrentTurn() == owner)
                {
                    return true;
                }

                return false;

            case SpellSpeedType.SpellSpeed2:


                return false;

            case SpellSpeedType.SpellSpeed3:


                return false;

            default:

                return false;
        }
    }

    public virtual bool ConditionsForMonster()
    {
        foreach (ConditionDefault condition in conditionEffectsList)
        {
            if (!condition.Condtions())
            {
                return false;
            }
        }

        return true;
    }

    public virtual IEnumerator Activation()
    {
        foreach (SpellCardEffect effect in activationEffectsList)
        {
            yield return StartCoroutine(effect.Resolve());
        }
    }

    public virtual IEnumerator Resolve()
    {
        yield return StartCoroutine(Activation());

        foreach (SpellCardEffect effect in resolveEffectsList)
        {
            yield return StartCoroutine(effect.Resolve());

            if (!GetStateEffect(effect))
            {
                if (effect.GetEffectResult())
                {
                    continue;
                }
            }

            else
            {
                continue;
            }

            break;
        }

        if ((cardSO is SpellCardSO))
        {
            if ((cardSO as SpellCardSO).spellCardType == SpellCardType.Normal)
            {
                yield return StartCoroutine(card.SendCardToGraveyard());
            }
        }

        if ((cardSO is MonsterCardSO))
        {
            foreach (SpellCardEffect effect in GetResolveCardEffects())
            {
                effect.ResetValues();
            }

            foreach (SpellCardEffect effect in GetActivationCardEffects())
            {
                effect.ResetValues();
            }
        }
    }

    public virtual IEnumerator BehaveAfterDestroyed()
    {
        switch (equipStateEffect)
        {
            case EquipStateEffect.None:
                break;
            case EquipStateEffect.DestroyIfGotDestroyedOrReturnToHandOrDeck:

                yield break;

            case EquipStateEffect.StayRemaining:
                break;
            default:
                break;
        }
    }
}
