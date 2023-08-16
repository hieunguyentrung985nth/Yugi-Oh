using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckZone : MonoBehaviour
{
    [SerializeField] private DeckSO deckSO;

    [SerializeField] private Transform shuffleDeckTransform;

    [SerializeField] private TextMeshPro deckText;

    [SerializeField] private Transform container;

    private Character owner;

    private float timeShuffleDeck = 1f;

    private List<Card> cardsList;

    private void Awake()
    {
        cardsList = new List<Card>();

        LeanTween.init(1000);
    }

    private void Start()
    {
        shuffleDeckTransform.gameObject.SetActive(false);

        deckText.text = "0";
    }

    public void Initialize()
    {
        foreach (CardSO cardSO in deckSO.cards)
        {
            CreateAndAddCard(cardSO);
        }

        RepositionCards();

        //ShuffleDeck();
    }

    private void CreateAndAddCard(CardSO cardSO)
    {
        Card newCard = Instantiate(cardSO.cardPrefab, container).GetComponent<Card>();

        newCard.SetUp();

        newCard.SetOwner(owner);

        cardsList.Add(newCard);

        newCard.GetCardVisual().FaceDownOnField();
    }

    public void AddCard(Card card)
    {
        cardsList.Add(card);

        RepositionCards();
    }

    private void RepositionCards()
    {
        if (owner == AI.Instance)
        {
            for (int i = 0; i < cardsList.Count; i++)
            {
                LeanTween.move(cardsList[i].gameObject, owner.GetDeckZone().GetContainer().transform.position + new Vector3(0f, 0f, i * Card.offsetZCard * 2f), 0f)
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
                LeanTween.move(cardsList[i].gameObject, owner.GetDeckZone().GetContainer().transform.position + new Vector3(0f, 0f, i * Card.offsetZCard), 0f)
               .setFrom(transform.position)
               .setOnComplete(() =>
               {

               });
            }
        }

        deckText.text = cardsList.Count.ToString();
    }

    public Card RemoveCard(Card card)
    {
        cardsList.Remove(card);

        RepositionCards();

        return card;
    }

    public List<Card> GetDeckCard()
    {
        return cardsList;
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < cardsList.Count; i++)
        {
            int randomIndex = Random.Range(i, cardsList.Count);

            Card temp = cardsList[randomIndex];

            cardsList[randomIndex] = cardsList[i];

            cardsList[i] = temp;
        }
    }

    public IEnumerator ShuffleDeckAnimation()
    {
        ShuffleDeck();

        shuffleDeckTransform.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeShuffleDeck);

        shuffleDeckTransform.gameObject.SetActive(false);
    }

    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }

    public Transform GetContainer()
    {
        return container;
    }
}
