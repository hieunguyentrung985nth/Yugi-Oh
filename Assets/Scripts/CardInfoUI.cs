using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoUI : MonoBehaviour
{
    public static CardInfoUI Instance { get; private set; }

    [Serializable]
    public class MonsterTypeDictionary
    {
        public MonsterType monsterType;

        public string translateText;
    }

    [SerializeField] private List<MonsterTypeDictionary> monsterTypeDictionary;

    [SerializeField] private Sprite defaultImage;

    [SerializeField] private Image cardImage;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private float timeText;

    private float timeDelayMax = 0.5f;

    private float timeDelay;

    private bool isLock;

    private float timeLockMax = 1f;

    private float timeLock;

    private bool startCountdown;

    private Card currentCard;

    private Card nextCard;

    private CardSO currentCardSO;

    private bool newCard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EmptyCard();
    }

    private void Update()
    {
        CountdownBeforeShowing();
    }

    private void EmptyCard()
    {
        titleText.text = "";

        typeText.text = "";

        descriptionText.text = "";

        cardImage.sprite = defaultImage;
    }

    private IEnumerator ShowCard()
    {
        EmptyCard();

        currentCardSO = currentCard.GetCardSO();

        cardImage.sprite = currentCardSO.image;

        string typeTextRes = null;

        if (currentCardSO is MonsterCardSO)
        {
            typeTextRes = "[" + GetMonsterType((currentCardSO as MonsterCardSO).monsterType) + "]";

            typeText.gameObject.SetActive(true);
        }

        else
        {
            typeTextRes = null;

            typeText.gameObject.SetActive(false);
        }

        yield return StartCoroutine(PlayText(currentCardSO.cardName, titleText));

        yield return StartCoroutine(PlayText(typeTextRes, typeText));

        yield return StartCoroutine(PlayText(currentCardSO.description, descriptionText));
    }

    private IEnumerator PlayText(string value, TextMeshProUGUI textUI)
    {
        if (value == null)
        {
            yield break;
        }

        int currentIndex = 0;

        string text = "";

        while (currentIndex < value.Length)
        {
            if (currentIndex < value.Length - 1)
            {
                currentIndex += 2;
            }

            else
            {
                currentIndex += 1;
            }

            text = value.Substring(0, currentIndex);

            text += "<color=#00000000>" + value.Substring(currentIndex) + "</color>";

            textUI.text = text;

            yield return new WaitForSeconds(timeText);
        }
    }

    private string GetMonsterType(MonsterType monsterType)
    {
        foreach (MonsterTypeDictionary type in monsterTypeDictionary)
        {
            if (type.monsterType == monsterType)
            {
                return type.translateText;
            }
        }

        return null;
    }

    public void StartHovering()
    {
        if (nextCard.IsCardOnDeck())
        {
            currentCard = null;

            newCard = true;
        }

        else if (nextCard.GetOwner() == AI.Instance)
        {
            if (nextCard.IsFaceUp())
            {
                if (currentCard == null)
                {
                    currentCard = nextCard;

                    newCard = true;
                }

                else if (currentCard.GetCardSO() != nextCard.GetCardSO())
                {
                    currentCard = nextCard;

                    newCard = true;
                }

                else
                {
                    newCard = false;
                }
            }

            else
            {
                currentCard = null;

                nextCard = null;

                newCard = true;
            }
        }

        else
        {
            if (currentCard == null)
            {
                currentCard = nextCard;

                newCard = true;
            }

            else if (currentCard.GetCardSO() != nextCard.GetCardSO())
            {
                currentCard = nextCard;

                newCard = true;
            }

            else
            {
                newCard = false;
            }
        }

        SetCard();
    }

    private void SetCard()
    {
        if (currentCard == null)
        {
            currentCardSO = null;

            EmptyCard();
        }

        else if (!isLock)
        {
            if (newCard)
            {
                StopAllCoroutines();

                StartCoroutine(ShowCard());
            }
        }
    }

    public void ExitHovering()
    {
        startCountdown = false;

        timeDelay = 0f;

        newCard = false;

        this.nextCard = null;
    }

    public void ShowCardImmediately(Card card)
    {
        ShowCardInfo(card);

        StartHovering();

        isLock = true;

        timeDelay = 0f;

        startCountdown = false;

        this.nextCard = null;
    }

    public void ShowCardInfo(Card card)
    {
        this.nextCard = card;

        startCountdown = true;
    }

    private void CountdownBeforeShowing()
    {
        if (startCountdown && !isLock)
        {
            timeDelay += Time.deltaTime;

            if (timeDelay >= timeDelayMax)
            {
                timeDelay = 0f;

                StartHovering();

                startCountdown = false;
            }
        }   

        else if (isLock)
        {
            timeLock += Time.deltaTime;

            if (timeLock >= timeLockMax)
            {
                timeLock = 0f;

                isLock = false;
            }
        }
    }
}
