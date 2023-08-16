using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AIReact;

public class AIReact : MonoBehaviour
{
    public static AIReact Instance { get; private set; }

    [SerializeField] private AIReactionForSpecialCardSO specialReactionSO;

    [SerializeField] private AIReactionVoiceLineSO voiceLineSO;

    [SerializeField] private AIVoiceUI aIVoiceUI;

    private AIReactionVoiceLine aIReactionVoiceLine;

    private AIReactionVoiceLineForSpecialCards aIReactionVoiceLineForSpecialCards;

    bool haveHurt5;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PointText.OnDecreasePoint += PointText_OnDecreasePoint;

        AI.Instance.OnAIPlayMonsterCardFromHand += AI_OnAIPlayMonsterCardFromHand;

        aIVoiceUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PointText.OnDecreasePoint -= PointText_OnDecreasePoint;
    }

    private void AI_OnAIPlayMonsterCardFromHand(object sender, AI.OnAIPlayMonsterCardFromHandEventArgs e)
    {
        aIReactionVoiceLineForSpecialCards = GetAIReactionVoiceLineForSpecialCards(e.card.GetCardSO());
    }

    private void PointText_OnDecreasePoint(object sender, PointText.OnDecreasePointEventArgs e)
    {
        if (e.pointDecrease == 0)
        {
            return;
        }

        if (e.owner == Player.Instance)
        {
            return;
        }

        if (AI.Instance.GetLifePoints() >= 4000)
        {
            if (e.pointDecrease <= 1000)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt1);
            }

            else if (e.pointDecrease <= 1500)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt2);
            }

            else if(e.pointDecrease <= 2000)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt3);
            }

            else
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt4);
            }
        }
        
        else
        {
            if (AI.Instance.GetLifePoints() - e.pointDecrease <= 0)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Lost);
            }

            else if (e.pointDecrease <= 1000)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt4);
            }

            else if (e.pointDecrease <= 2000)
            {
                aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt3);
            }

            else if(e.pointDecrease > 2000 && !haveHurt5)
            {
                if(AI.Instance.GetLifePoints() - e.pointDecrease <= PointSingle.dangerPoints && AI.Instance.GetLifePoints() > 2500)
                {
                    aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt5);

                    haveHurt5 = true;
                }    
                
                else
                {
                    aIReactionVoiceLine = GetAIReactionVoiceLine(AIReaction.Hurt4);
                }
            }
        }

        StartCoroutine(AIReactionVoiceLine());
    }

    public IEnumerator AIReactionVoiceLine(AIReaction reaction = default, bool random = false)
    {
        if (reaction != default)
        {
            this.aIReactionVoiceLine = GetAIReactionVoiceLine(reaction);
        }

        if (aIReactionVoiceLine == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.3f);

        aIVoiceUI.gameObject.SetActive(true);

        switch (aIReactionVoiceLine.reactionBehavior)
        {
            case AIReactionBehavior.WaitTillFinish:

                Time.timeScale = 0f;

                break;

            case AIReactionBehavior.SameTimeWithAction:
                break;
            case AIReactionBehavior.TalkedFirst:



                break;

            case AIReactionBehavior.AfterPlay:
                break;
            case AIReactionBehavior.AfterTrapFlipped:
                break;
            default:
                break;
        }

        if (!random)
        {
            yield return StartCoroutine(aIVoiceUI.SetText(aIReactionVoiceLine.voiceLines));
        }

        else
        {
            List<string> voiceLines = GetVoiceLine(reaction);

            yield return StartCoroutine(aIVoiceUI.SetText(voiceLines));
        }

        aIVoiceUI.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    public IEnumerator AIReactionForSpecialCards(Card card, AIReaction reaction)
    {
        aIReactionVoiceLineForSpecialCards = GetAIReactionVoiceLineForSpecialCards(card.GetCardSO());

        aIReactionVoiceLine = GetAIReactionVoiceLine(reaction);

        List<string> voiceLines = HandleVoiceLine(card);

        if (voiceLines.Count == 0)
        {
            yield break;
        }

        aIVoiceUI.gameObject.SetActive(true);

        switch (aIReactionVoiceLine.reactionBehavior)
        {
            case AIReactionBehavior.WaitTillFinish:

                Time.timeScale = 0f;

                break;

            case AIReactionBehavior.SameTimeWithAction:
                break;
            case AIReactionBehavior.TalkedFirst:



                break;

            case AIReactionBehavior.AfterPlay:
                break;
            case AIReactionBehavior.AfterTrapFlipped:
                break;
            default:
                break;
        }

        yield return StartCoroutine(aIVoiceUI.SetText(voiceLines));

        aIVoiceUI.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    private List<string> HandleVoiceLine(Card card)
    {
        List<string> voiceLines = new List<string>();

        if (card is MonsterCard)
        {          
            MonsterCard monsterCard = (MonsterCard)card;

            if (monsterCard.NumberForTribute() > 0)
            {               
                if (aIReactionVoiceLineForSpecialCards != null)
                {
                    if (monsterCard.NumberForTribute() == 1)
                    {
                        voiceLines.AddRange(GetVoiceLine(AIReaction.SummonMonsterWithTribute));
                    }

                    voiceLines.AddRange(aIReactionVoiceLineForSpecialCards.voiceLines);
                }

                else
                {
                    voiceLines.AddRange(GetVoiceLine(AIReaction.SummonMonsterWithTribute));

                    if (monsterCard.NumberForTribute() == 1)
                    {
                        voiceLines.AddRange(GetVoiceLineForSpecialCards(AIReaction.SummonMonsterWithOneTribute));
                    }

                    else if (monsterCard.NumberForTribute() == 2)
                    {
                        voiceLines.AddRange(GetVoiceLineForSpecialCards(AIReaction.SummonMOnsterWithTwoTribute));
                    }
                }              
            }  
            
            else
            {
                if (aIReactionVoiceLineForSpecialCards != null)
                {
                    voiceLines.AddRange(aIReactionVoiceLineForSpecialCards.voiceLines);
                }
            }
        }

        return voiceLines;
    }

    public List<string> GetVoiceLine(AIReaction aIreaction)
    {
        List<AIReactionVoiceLine> list = new List<AIReactionVoiceLine>();

        foreach (AIReactionVoiceLine reactionVoiceLine in voiceLineSO.aIReactionVoiceLines) 
        {
            if (reactionVoiceLine.reaction == aIreaction)
            {
                list.Add(reactionVoiceLine);
            }
        }

        int r = UnityEngine.Random.Range(0, list.Count);

        return list[r].voiceLines;
    }

    public List<string> GetVoiceLineForSpecialCards(CardSO cardSO)
    {
        foreach (AIReactionVoiceLineForSpecialCards reactionVoiceLine in specialReactionSO.aIReactionVoiceLineForSpecialCards)
        {
            if (reactionVoiceLine.cardSO == cardSO)
            {
                return reactionVoiceLine.voiceLines;
            }
        }

        return null;
    }

    public List<string> GetVoiceLineForSpecialCards(AIReaction reaction)
    {
        foreach (AIReactionVoiceLineForSpecialCards reactionVoiceLine in specialReactionSO.aIReactionVoiceLineForSpecialCards)
        {
            if (reactionVoiceLine.reaction == reaction)
            {
                return reactionVoiceLine.voiceLines;
            }
        }

        return null;
    }


    public AIReactionVoiceLineForSpecialCards GetAIReactionVoiceLineForSpecialCards(CardSO cardSO)
    {
        foreach (AIReactionVoiceLineForSpecialCards reactionVoiceLine in specialReactionSO.aIReactionVoiceLineForSpecialCards)
        {
            if (reactionVoiceLine.cardSO == cardSO)
            {
                return reactionVoiceLine;
            }
        }

        return null;
    }

    public AIReactionVoiceLine GetAIReactionVoiceLine(AIReaction aIreaction)
    {
        foreach (AIReactionVoiceLine reactionVoiceLine in voiceLineSO.aIReactionVoiceLines)
        {
            if (reactionVoiceLine.reaction == aIreaction)
            {
                return reactionVoiceLine;
            }
        }

        return null;
    }
}
