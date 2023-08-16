using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuTutorial : MonoBehaviour
{
    [SerializeField] private MenuTextSO menuText;

    [SerializeField] private Button backButton;

    [SerializeField] private Button nextButton;

    [SerializeField] private Button confirmButton;

    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private TextMeshProUGUI pageText;

    private int currentPage = 1;

    private void Awake()
    {
        backButton.onClick.AddListener(PreviousPage);

        nextButton.onClick.AddListener(NextPage);

        confirmButton.onClick.AddListener(StartGame);
    }

    private void Start()
    {
        SetContent();
    }

    private void SetContent()
    {
        contentText.text = menuText.texts[currentPage - 1];

        pageText.text = string.Format("{0}/{1}", currentPage, menuText.texts.Count);

        if(currentPage == menuText.texts.Count)
        {
            HideNextButton();

            ShowBackButton();

            ShowConfirmButton();
        }

        else
        {
            HideConfirmButton();

            HideBackButton();

            ShowNextButton();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void NextPage()
    {
        currentPage++;

        SetContent();
    }

    private void PreviousPage()
    {
        currentPage--;

        SetContent();
    }

    private void ShowConfirmButton()
    {
        confirmButton.gameObject.SetActive(true);
    }

    private void HideConfirmButton()
    {
        confirmButton.gameObject.SetActive(false);
    }

    private void HideBackButton()
    {
        backButton.gameObject.SetActive(false);
    }

    private void ShowBackButton()
    {
        backButton.gameObject.SetActive(true);
    }

    private void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    private void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }
}
