using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhasePopupUI : MonoBehaviour
{
    public static PhasePopupUI Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI phaseText;

    [SerializeField] private float timeFade;

    [SerializeField] private float timeShow;

    [SerializeField] private float sizeTextChangeTurn;

    [SerializeField] private float sizeTextNormal;

    [SerializeField] private Color32 playerColor;

    [SerializeField] private Color32 aIColor;

    [Header("Phase Text")]
    [SerializeField] private string drawPhase;

    [SerializeField] private string standbyPhase;

    [SerializeField] private string mainPhase1;

    [SerializeField] private string battlePhase;

    [SerializeField] private string mainPhase2;

    [SerializeField] private string endPhase;

    [SerializeField] private string endTurn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhaseManager.Instance.OnPhaseChange += PhaseManager_OnPhaseChange;

        PhaseManager.Instance.OnEndPhase += PhaseManager_OnEndPhase;

        //TurnManager.Instance.OnChangeTurn += TurnManager_OnChangeTurn;

        Hide();
    }

    private void TurnManager_OnChangeTurn(object sender, System.EventArgs e)
    {
        Hide();

        Show();

        SetUp(default, true);

        StartCoroutine(StartTransition());
    }

    private void PhaseManager_OnEndPhase(object sender, System.EventArgs e)
    {
        Hide();

        LeanTween.cancel(gameObject);

        Show();

        SetUp(default, true);

        StartCoroutine(StartTransition());
    }

    private void PhaseManager_OnPhaseChange(object sender, PhaseManager.OnPhaseChangeEventArgs e)
    {
        Hide();

        LeanTween.cancel(gameObject);

        Show();

        SetUp(e.phase, false);

        StartCoroutine(StartTransition());
    }

    public IEnumerator StartTransition()
    {
        LeanTween.alphaCanvas(canvasGroup, 1f, timeFade)
            .setOnComplete(() =>
            {
                
            });

        yield return new WaitForSeconds(timeShow);

        LeanTween.alphaCanvas(canvasGroup, 0f, timeFade)
            .setOnComplete(() =>
            {
                Hide();
            });
    }

    private void SetUp(PhaseManager.Phase phase = default, bool endTurn = false)
    {
        if (!endTurn)
        {
            phaseText.fontSize = sizeTextNormal;

            switch (phase)
            {
                case PhaseManager.Phase.Draw:

                    phaseText.text = drawPhase;

                    break;
                case PhaseManager.Phase.Standby:

                    phaseText.text = standbyPhase;

                    break;
                case PhaseManager.Phase.Main1:

                    phaseText.text = mainPhase1;

                    break;
                case PhaseManager.Phase.Battle:

                    phaseText.text = battlePhase;

                    break;
                case PhaseManager.Phase.Main2:

                    phaseText.text = mainPhase2;

                    break;
                case PhaseManager.Phase.End:

                    phaseText.text = endPhase;

                    break;

                default:
                    break;
            }
        }

        else
        {
            phaseText.fontSize = sizeTextChangeTurn;

            phaseText.text = this.endTurn;
        }

        if (TurnManager.Instance.IsPlayerTurn())
        {
            phaseText.color = playerColor;
        }

        else
        {
            phaseText.color = aIColor;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        canvasGroup.alpha = 0f;
    }
}
