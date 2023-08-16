using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoxUI : MonoBehaviour
{
    public static SelectBoxUI Instance { get; private set; }

    [SerializeField] private Transform container;

    [SerializeField] private Transform template;

    private List<SelectBoxUISingle> cardsList;

    private float timeBeforeShow = 1f;

    private void Awake()
    {
        Instance = this;

        cardsList = new List<SelectBoxUISingle>();
    }

    private void Start()
    {
        Hide();
    }

    public IEnumerator SetUp(List<Card> cardsList)
    {
        ClearList();

        foreach (Card card in cardsList)
        {
            GameObject cardObject = Instantiate(template.gameObject, container);

            SelectBoxUISingle selectBoxUISingle = cardObject.GetComponent<SelectBoxUISingle>();

            selectBoxUISingle.SetUp(card);

            selectBoxUISingle.TurnOnSelectable();

            cardObject.SetActive(true);

            this.cardsList.Add(selectBoxUISingle);
        }

        yield return new WaitForSeconds(timeBeforeShow);

        Show();
    }

    public void ClearList()
    {
        foreach (Transform child in container.transform)
        {
            if (child == template) continue;

            Destroy(child.gameObject);
        }
    }

    public void TurnOffSelectable()
    {
        foreach (SelectBoxUISingle single in cardsList)
        {
            single.TurnOffSelectable();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
