using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int STARTING_HAND_SIZE = 5;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStartDrawPhase;

    public event EventHandler OnGameOver;

    [SerializeField] private Transform effectObject;

    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform startGameUITransform;

    [SerializeField] private Transform endGameUITransform;

    [SerializeField] private Transform blockRaycastTransform;

    [SerializeField] private Button playAgainButton;

    [SerializeField] private Vector3 start;

    [SerializeField] private Vector3 normal;

    [SerializeField] private float timeTransition;

    [SerializeField] private float timeDelay;

    [SerializeField] private string youWin;

    [SerializeField] private string youLose;

    [SerializeField] private Color32 youWinColor;

    [SerializeField] private Color32 youLoseColor;

    private bool isGameStart;

    private float timeShowMax = 2f;

    private float timeShow;

    private bool isPlayerWin;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        TurnManager.Instance.OnStartGame += TurnManager_OnStartGame;

        BattleSystem.Instance.OnCheckGameOver += BattleSystem_OnCheckGameOver;

        canvas.gameObject.SetActive(false);

        LeanTween.scale(startGameUITransform.GetComponentInChildren<TextMeshProUGUI>().gameObject, start, 0f);

        startGameUITransform.gameObject.SetActive(false);

        blockRaycastTransform.gameObject.SetActive(false);
    }

    private void BattleSystem_OnCheckGameOver(object sender, EventArgs e)
    {
        if (AI.Instance.GetLifePoints() == 0)
        {
            StartCoroutine(EndGame(true));
        }

        else if(Player.Instance.GetLifePoints() == 0)
        {
            StartCoroutine(EndGame(false));
        }
    }

    private void Update()
    {
        StartGame();
    }

    private void TurnManager_OnStartGame(object sender, System.EventArgs e)
    {
        isGameStart = true;

        canvas.gameObject.SetActive(true);

        StartCoroutine(StartDuelAnimation());
    }

    private IEnumerator StartDrawingFirstTime()
    {
        yield return new WaitForSeconds(timeDelay);

        yield return StartCoroutine(TurnManager.Instance.GetCurrentTurn().DrawCard(STARTING_HAND_SIZE));

        yield return StartCoroutine(TurnManager.Instance.GetNotCurrentTurn().DrawCard(STARTING_HAND_SIZE));

        OnStartDrawPhase?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetEffectTransform()
    {
        return effectObject;
    }

    private IEnumerator StartDuelAnimation()
    {
        yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.StartGame, true));

        yield return new WaitForSeconds(timeDelay);

        startGameUITransform.gameObject.SetActive(true);

        startGameUITransform.GetChild(0).gameObject.SetActive(true);

        LeanTween.alphaCanvas(startGameUITransform.GetComponent<CanvasGroup>(), 1f, timeTransition)
            .setOnComplete(() =>
            {
                startGameUITransform.GetChild(1).gameObject.SetActive(true);

                LeanTween.scale(startGameUITransform.GetComponentInChildren<TextMeshProUGUI>().gameObject, normal, timeTransition)
                .setOnComplete(() =>
                {
                    canvas.GetComponent<GraphicRaycaster>().enabled = false;
                });
            });    
    }

    private void StartGame()
    {
        if (isGameStart)
        {
            timeShow += Time.deltaTime;

            if (timeShow >= timeShowMax)
            {
                timeShow = 0f;

                isGameStart = false;

                LeanTween.alphaCanvas(startGameUITransform.GetComponent<CanvasGroup>(), 0f, timeTransition)
                    .setOnComplete(() =>
                    {
                        startGameUITransform.gameObject.SetActive(false);
                    });

                StartCoroutine(StartDrawingFirstTime());
            }
        }
    }

    private IEnumerator EndGameAnimation()
    {
        if (isPlayerWin)
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.LoseGame, true));
        }

        else
        {
            yield return StartCoroutine(AIReact.Instance.AIReactionVoiceLine(AIReaction.WinGame, true));
        }

        yield return new WaitForSecondsRealtime(timeDelay);

        endGameUITransform.gameObject.SetActive(true);

        endGameUITransform.GetChild(0).gameObject.SetActive(true);

        LeanTween.alphaCanvas(endGameUITransform.GetComponent<CanvasGroup>(), 1f, timeTransition)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                endGameUITransform.GetChild(1).gameObject.SetActive(true);

                LeanTween.alphaCanvas(endGameUITransform.GetComponentInChildren<TextMeshProUGUI>().GetComponent<CanvasGroup>(), 1f, timeTransition)
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    playAgainButton.gameObject.SetActive(true);

                    canvas.GetComponent<GraphicRaycaster>().enabled = true;
                });
            });
    }

    private IEnumerator EndGame(bool playerWin)
    {
        this.isPlayerWin = playerWin;

        if (playerWin)
        {
            endGameUITransform.GetComponentInChildren<TextMeshProUGUI>().text = youWin;

            endGameUITransform.GetComponentInChildren<TextMeshProUGUI>().color = youWinColor;
        }

        else
        {
            endGameUITransform.GetComponentInChildren<TextMeshProUGUI>().text = youLose;

            endGameUITransform.GetComponentInChildren<TextMeshProUGUI>().color = youLoseColor;
        }

        OnGameOver?.Invoke(this, EventArgs.Empty);

        blockRaycastTransform.gameObject.SetActive(true);

        yield return StartCoroutine(EndGameAnimation());
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
