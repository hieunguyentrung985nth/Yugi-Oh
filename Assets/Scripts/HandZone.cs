using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandZone : MonoBehaviour
{
    private const float SPACING_CARDS_5 = 2f;

    private const float SPACING_CARDS_6_9 = 1.5f;

    private const float SPACING_CARDS_10 = 1f;

    [SerializeField] private List<Card> cards;

    private HorizontalLayoutGroup horizontalLayoutGroup;

    private Transform cardStartPosition;

    private float cardOffset = 2f;

    public float maxOverlap = 2f; 

    public int maxCards = 9;

    private float moveTimer = 0f;

    private float moveTimerMax = 0.5f;

    private void Awake()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

        cards = new List<Card>();
        
        cardStartPosition = transform;
    }

    private void Start()
    {
        Character.OnHandChanged += Character_OnHandChanged;
    }

    private void OnDestroy()
    {
        Character.OnHandChanged -= Character_OnHandChanged;
    }

    private void Character_OnHandChanged(object sender, System.EventArgs e)
    {
        (sender as Character).StartCoroutine(RepositionAfterCardDraw());
    }

    public IEnumerator AddCard(Card card)
    {
        cards.Add(card);

        yield return StartCoroutine(RepositionAfterCardDraw(card));
    }

    public IEnumerator RepositionAfterCardDraw(Card card = null)
    {
        int handSize = cards.Count;

        Vector3 targetPosition = new Vector3(0f, 0f, 0f);

        float xOffset = (handSize - 1) * cardOffset / 2f;

        if (handSize <= 1)
        {
            targetPosition = cardStartPosition.position;
        }
        else
        {
            targetPosition = cards[handSize - 2].transform.position;
        }

        if (card != null)
        {
            yield return StartCoroutine(MoveCardToPosition(card, targetPosition));
        }

        if (0 <= handSize && handSize <= 5)
        {
            horizontalLayoutGroup.spacing = SPACING_CARDS_5;
        }

        else if (6 <= handSize && handSize <= 9)
        {
            horizontalLayoutGroup.spacing = SPACING_CARDS_6_9;
        }

        else
        {
            horizontalLayoutGroup.spacing = SPACING_CARDS_10;
        }

        //if (handSize >= 0 && handSize < 8)
        //{
        //    for (int i = 0; i < cards.Count; i++)
        //    {
        //        Vector3 leftCardPosition = cards[i].transform.position - new Vector3(cardOffset / 2f, 0f, 0f);

        //        StartCoroutine(MoveCardToPosition(cards[i], leftCardPosition));
        //    }

        //    StartCoroutine(MoveCardToPosition(card, cardPosition));
        //}

        //if (handSize >= 8)
        //{
        //    Vector3 rightCardPosition = new Vector3(0f, 0f, 0f);

        //    for (int i = 1; i < cards.Count - 1; i++)
        //    {
        //        Vector3 overlapPosition = cards[i].transform.position - new Vector3(i * cardOffset / handSize, 0f, 0f);

        //        StartCoroutine(MoveCardToPosition(cards[i], overlapPosition));

        //        if (i == cards.Count - 2)
        //        {
        //            rightCardPosition = overlapPosition + new Vector3(i * cardOffset / handSize, 0f, 0f);
        //        }
        //    }

        //    StartCoroutine(MoveCardToPosition(card, rightCardPosition));
        //}
    }

    private IEnumerator MoveCardToPosition(Card card, Vector3 targetPosition)
    {
        Vector3 startPos = card.transform.position;

        while (moveTimer < moveTimerMax)
        {
            moveTimer += Time.deltaTime;

            float t = Mathf.Clamp01(moveTimer / moveTimerMax);

            card.transform.position = Vector3.Lerp(startPos, targetPosition, t);

            yield return null;
        }

        card.transform.position = targetPosition;

        moveTimer = 0;

        card.transform.SetParent(transform);

        card.SetHorizontalLayoutGroup(horizontalLayoutGroup);

        card.UpdateCardState(CardState.Hand);
    }


    public void RemoveCard(Card card)
    {
        cards.Remove(card);

        RepositionAfterCardDraw();
    }

    public List<Card> GetHandCard()
    {
        return cards;
    }

    public List<MonsterCard> GetMonsterCardsOnHand()
    {
        List<MonsterCard> list = new List<MonsterCard>();

        foreach (Card card in cards)
        {
            if (card is MonsterCard)
            {
                list.Add(card as MonsterCard);
            }
        }

        return list;
    }

    private void RepositionCards()
    {
        float cardWidth = 200f;

        float totalWidth = cardWidth * cards.Count;

        float startX = -totalWidth / 2f;

        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform cardTransform = cards[i].GetComponentsInChildren<RectTransform>()[1];

            float x = startX + i * cardWidth;
            cardTransform.anchoredPosition = new Vector2(x, 0f);

            cardTransform.parent.GetComponent<Canvas>().sortingOrder = i;
        }
    }
}
