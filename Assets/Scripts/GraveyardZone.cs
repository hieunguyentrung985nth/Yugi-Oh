using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraveyardZone : MonoBehaviour
{
    [SerializeField] private TextMeshPro graveyardText;

    [SerializeField] private Transform container;

    private Character owner;

    private List<Card> cardsList;

    private float timeMoveToTop = 0.2f;

    private void Awake()
    {
        cardsList = new List<Card>();
    }

    private void Start()
    {
        graveyardText.text = cardsList.Count.ToString();
    }

    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }

    public void AddCard(Card card)
    {
        cardsList.Add(card);

        card.transform.SetParent(container);

        RepositionCards();

        graveyardText.text = cardsList.Count.ToString();
    }

    public void RemoveCard(Card card)
    {
        cardsList.Remove(card);

        RepositionCards();

        graveyardText.text = cardsList.Count.ToString();
    }

    private void RepositionCards()
    {
        if (owner == AI.Instance)
        {
            for (int i = 0; i < cardsList.Count; i++)
            {
                LeanTween.move(cardsList[i].gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, i * Card.offsetZCard * 2f), 0f)
               .setFrom(transform.position)
               .setOnComplete(() =>
               {

               });
            }
        }
        
        else
        {
            for (int i = 0; i < cardsList.Count; i++)
            {
                LeanTween.move(cardsList[i].gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, i * Card.offsetZCard), 0f)
               .setFrom(transform.position)
               .setOnComplete(() =>
               {

               });
            }
        }
    }

    public IEnumerator MoveCardToTheTop(Card card)
    {
        if (!(cardsList[cardsList.Count - 1] == card))
        {
            LeanTween.move(card.gameObject, card.transform.position + transform.right * 2f, timeMoveToTop)
            .setFrom(card.transform.position)
            .setOnComplete(() =>
            {
                cardsList.Remove(card);

                card.GetCardVisual().GetSpriteRenderer().sortingOrder = 1;

                RepositionCards();

                if (owner == AI.Instance)
                {
                    LeanTween.move(card.gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, (cardsList.Count - 1) * Card.offsetZCard * 2f), timeMoveToTop)
                        .setFrom(card.transform.position)
                        .setOnComplete(() =>
                        {
                            cardsList.Add(card);

                            RepositionCards();

                            card.GetCardVisual().GetSpriteRenderer().sortingOrder = 0;

                        });
                }

                else
                {
                    LeanTween.move(card.gameObject, owner.GetGraveyardZone().GetContainer().transform.position + new Vector3(0f, 0f, (cardsList.Count - 1) * Card.offsetZCard), timeMoveToTop)
                        .setFrom(card.transform.position)
                        .setOnComplete(() =>
                        {
                            cardsList.Add(card);

                            RepositionCards();

                            card.GetCardVisual().GetSpriteRenderer().sortingOrder = 0;

                        });
                }
                
            });

            yield return new WaitForSeconds(timeMoveToTop * 3);
        }
    }

    private void SwapPlace(Card card)
    {
        cardsList.Remove(card);

        cardsList.Add(card);
    }

    public List<Card> GetGraveyardCards()
    {
        return cardsList;
    }

    public List<MonsterCard> GetMonsterCardsInGraveyard()
    {
        List<MonsterCard> list = new List<MonsterCard>();

        foreach (Card card in cardsList)
        {
            if (card is MonsterCard)
            {
                list.Add(card as MonsterCard);
            }
        }

        return list;
    }

    public Transform GetContainer()
    {
        return container;
    }
}
