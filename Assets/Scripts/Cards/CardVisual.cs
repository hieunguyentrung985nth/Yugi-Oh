using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    private const float TRANSITION_TIME_ROTATE_CARD = 0.5f;

    private const float TRANSITION_TIME_ROTATE_CARD_FOR_TRIBUTE = 1f;

    private const float TRANSITION_TIME_ROTATE_CARD_FOR_FLIP = 0.15f;

    private const float TRANSITION_TIME_CARD_CHANGE_COLOR_ACTIVE = 0.5f;

    private const float TRANSITION_TIME_CARD_EQUIP = 0.75f;

    private const float TRANSITION_TIME_SPECIAL_SUMMON = 1f;

    private const float DELAY_TIME_AFTER_SPECIAL_SUMMON = 0.25f;

    private const int TIME_FOR_REVEAL = 8;

    private const int TIME_FOR_SELECT_TARGET = 8;

    private const int TIME_FOR_CARD_BE_ATTACKED = 4;

    [SerializeField] private Card card;

    [SerializeField] private SpriteRenderer cardFrontImageOnField;

    [SerializeField] private SpriteRenderer cardBackImageOnField;

    [SerializeField] private Image cardFrontImageOnHand;

    [SerializeField] private Image cardBackImageOnHand;

    [SerializeField] private Image cardFrontImageOnSelect;

    [SerializeField] private Image cardBackImageOnSelect;

    [SerializeField] private Material activeMaterialOnField;

    [SerializeField] private Material defaultMaterial;

    [SerializeField] private Material activeMaterialOnCanvas;

    [SerializeField] private Material markAsTribute;

    [SerializeField] private Material tribute;

    [SerializeField] private GameObject fakeCardObject;

    [SerializeField] private GameObject onHandCanvas;

    [SerializeField] private GameObject onFieldModel;

    [SerializeField] private GameObject onSelectCanvas;

    [SerializeField] private GameObject statusBar;

    [SerializeField] private Color32 activeColor;

    [SerializeField] private Color32 defaultColor;

    [SerializeField] private Color32 selectColor;

    [SerializeField] private Color32 playerBarColor;

    [SerializeField] private Color32 enemyBarColor;

    private float timeWaitingTributeAnimation = 1.25f;

    public void SetUpCardVisual(CardSO cardSO)
    {
        cardFrontImageOnField.sprite = cardSO.image;

        cardFrontImageOnHand.sprite = cardSO.image;

        cardFrontImageOnSelect.sprite = cardSO.image;

        cardFrontImageOnField.material = defaultMaterial;

        cardBackImageOnField.material = defaultMaterial;

        onHandCanvas.gameObject.SetActive(false);

        onSelectCanvas.gameObject.SetActive(false);

        SetUpForEquip(cardSO);
    }

    public void CardOnHand()
    {
        onHandCanvas.SetActive(true);

        onFieldModel.SetActive(false);

        onSelectCanvas.SetActive(false);
    }

    public void CardOnField()
    {
        onHandCanvas.SetActive(false);

        onFieldModel.SetActive(true);

        onSelectCanvas.SetActive(false);
    }

    public void FaceUpOnField()
    {
        CardOnField();

        cardFrontImageOnField.gameObject.SetActive(true);

        cardBackImageOnField.gameObject.SetActive(false);
    }

    public void FaceDownOnField()
    {
        CardOnField();

        cardFrontImageOnField.gameObject.SetActive(false);

        cardBackImageOnField.gameObject.SetActive(true);
    }

    public void FaceUpOnHand()
    {
        CardOnHand();

        cardFrontImageOnHand.gameObject.SetActive(true);

        cardBackImageOnHand.gameObject.SetActive(false);
    }

    public void FaceDownOnHand()
    {
        CardOnHand();

        cardFrontImageOnHand.gameObject.SetActive(false);

        cardBackImageOnHand.gameObject.SetActive(true);
    }

    public void CardOnSelect()
    {
        onHandCanvas.SetActive(false);

        onFieldModel.SetActive(false);

        onSelectCanvas.SetActive(true);
    }

    public void FaceUpOnSelect()
    {
        CardOnSelect();

        cardFrontImageOnSelect.gameObject.SetActive(true);

        cardBackImageOnSelect.gameObject.SetActive(false);
    }

    public void FaceDownOnSelect()
    {
        CardOnSelect();

        cardFrontImageOnSelect.gameObject.SetActive(false);

        cardBackImageOnSelect.gameObject.SetActive(true);
    }

    public IEnumerator FlipMonster()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(89f, transform.localEulerAngles.y, transform.localEulerAngles.z), TRANSITION_TIME_ROTATE_CARD_FOR_FLIP)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                FaceUpOnField(); 

                CardInfoUI.Instance.ShowCardImmediately(card);

                transform.localEulerAngles = new Vector3(-89f, transform.localEulerAngles.y, transform.localEulerAngles.z);

                LeanTween.rotateLocal(gameObject, new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z), TRANSITION_TIME_ROTATE_CARD_FOR_FLIP)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    
                });
            });

        yield return new WaitForSeconds(TRANSITION_TIME_ROTATE_CARD_FOR_FLIP * 2);
    }

    public IEnumerator FlipSpellTrap()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(transform.localEulerAngles.x, -90f, transform.localEulerAngles.z), TRANSITION_TIME_ROTATE_CARD_FOR_FLIP)
            .setOnComplete(() =>
            {
                FaceUpOnField();

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 90f, transform.localEulerAngles.z);

                LeanTween.rotateLocal(gameObject, new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z), TRANSITION_TIME_ROTATE_CARD_FOR_FLIP)
                .setOnComplete(() =>
                {

                });
            });

        yield return new WaitForSeconds(TRANSITION_TIME_ROTATE_CARD_FOR_FLIP * 2);
    }

    public void AttackPosition()
    {
        FaceUpOnField();
    }

    public void SetDefendFaceDownPosition()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -90f), TRANSITION_TIME_ROTATE_CARD);

        FaceDownOnField();
    }

    public void DefendFaceDownPosition()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -90f), 0f);

        FaceDownOnField();
    }

    public void DefendFaceUpPosition()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -90f), 0f);

        FaceDownOnField();
    }

    public void RotateToDefend()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -90f), TRANSITION_TIME_ROTATE_CARD);
    }

    public void RotateToAttack()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 0f), TRANSITION_TIME_ROTATE_CARD)
            .setOnComplete(() =>
            {
                FaceUpOnField();
            });
    }

    public void CardCanBePlayedOnHand()
    {
        cardFrontImageOnHand.material = activeMaterialOnCanvas;
    }

    public void CardNormalStateOnHand()
    {
        cardFrontImageOnHand.material = defaultMaterial;
    }

    public void CardActiveOnField()
    {
        cardFrontImageOnField.material = activeMaterialOnField;

        cardBackImageOnField.material = activeMaterialOnField;
    }

    public void CardActiveOnSelect()
    {
        cardFrontImageOnSelect.color = selectColor;

        cardBackImageOnSelect.color = selectColor;
    }

    public void CardNormalStateOnSelect()
    {
        cardFrontImageOnSelect.color = defaultColor;

        cardBackImageOnSelect.color = defaultColor;
    }

    private void SwitchSelectTargetOnHand()
    {
        cardFrontImageOnHand.material = cardFrontImageOnHand.material == defaultMaterial ? activeMaterialOnCanvas : defaultMaterial;
    }

    public IEnumerator CardBeAttacked()
    {
        int currentTime = 0;

        float transitionTimerMax = 0.05f;

        float transitionTimer = 0f;

        float timeMove = 0.01f;

        bool moveRight = true;

        LeanTween.moveLocal(gameObject, card.transform.right / 15f, timeMove);

        while (currentTime < TIME_FOR_CARD_BE_ATTACKED)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer >= transitionTimerMax)
            {
                transitionTimer = 0f;

                currentTime++;

                if (moveRight)
                {
                    LeanTween.moveLocal(gameObject, -card.transform.right * 2 / 15f, timeMove);
                }

                else
                {
                    LeanTween.moveLocal(gameObject, card.transform.right * 2 / 15f, timeMove);
                }

                moveRight = !moveRight;
            }

            yield return null;
        }

        LeanTween.moveLocal(gameObject, Vector3.zero, timeMove);
    }

    public IEnumerator RevealCardOnHand()
    {
        int currentTime = 0;

        float transitionTimerMax = 0.2f;

        float transitionTimer = 0f;

        onHandCanvas.GetComponent<CardHoverOnHand>().HoverCardOnHand();

        onHandCanvas.GetComponent<CardHoverOnHand>().TurnOffHovering();

        FaceUpOnHand();

        SwitchSelectTargetOnHand();

        CardInfoUI.Instance.ShowCardImmediately(card);

        while (currentTime < TIME_FOR_REVEAL)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer >= transitionTimerMax)
            {
                transitionTimer = 0f;

                currentTime++;

                SwitchSelectTargetOnHand();
            }

            yield return null;
        }

        CardNormalStateOnHand();

        onHandCanvas.GetComponent<CardHoverOnHand>().CardNormalOnHand();

        onHandCanvas.GetComponent<CardHoverOnHand>().TurnOnHovering();
    }

    public void CardNormalStateOnField()
    {
        cardFrontImageOnField.material = defaultMaterial;

        cardBackImageOnField.material = defaultMaterial;

        DisableMarkAsTribute();
    }

    public void DefaultCardOnField()
    {
        CardNormalStateOnField();

        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void MarkAsTribute()
    {
        fakeCardObject.gameObject.SetActive(true);

        fakeCardObject.GetComponent<SpriteRenderer>().material = markAsTribute;
    }

    public void DisableMarkAsTribute()
    {
        fakeCardObject.gameObject.SetActive(false);
    }

    public IEnumerator Tribute()
    {
        fakeCardObject.gameObject.SetActive(true);

        fakeCardObject.GetComponent<SpriteRenderer>().material = tribute;

        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 270f), TRANSITION_TIME_ROTATE_CARD_FOR_TRIBUTE / 3f)
            .setOnComplete(() =>
            {
                LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 180f), TRANSITION_TIME_ROTATE_CARD_FOR_TRIBUTE / 3f)
                    .setOnComplete(() =>
                    {
                        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 120f), TRANSITION_TIME_ROTATE_CARD_FOR_TRIBUTE / 3f)
                        .setOnComplete(() =>
                        {
                            fakeCardObject.gameObject.SetActive(false);
                        });
                    });
            });

        yield return new WaitForSeconds(timeWaitingTributeAnimation);
    }

    public IEnumerator ActiveEffect()
    {
        if (card.GetCardState() == CardState.Graveyard)
        {
            yield return StartCoroutine(card.GetOwner().GetGraveyardZone().MoveCardToTheTop(card));
        }

        LeanTween.color(gameObject, activeColor, TRANSITION_TIME_CARD_CHANGE_COLOR_ACTIVE)
            .setEase(LeanTweenType.easeOutQuint)
            .setOnComplete(() =>
            {
                LeanTween.color(gameObject, defaultColor, 0f)
                .setEase(LeanTweenType.easeOutQuint);
            });

        LeanTween.color(gameObject, activeColor, TRANSITION_TIME_CARD_CHANGE_COLOR_ACTIVE)
            .setEase(LeanTweenType.easeOutQuint)
            .setOnComplete(() =>
            {
                LeanTween.color(gameObject, defaultColor, 0f)
                .setEase(LeanTweenType.easeOutQuint)
                .setOnComplete(() =>
                {
                    
                });
            });

        CardInfoUI.Instance.ShowCardImmediately(card);

        yield return new WaitForSeconds(TRANSITION_TIME_CARD_CHANGE_COLOR_ACTIVE * 2);
    }

    public IEnumerator EquipCard(Transform target)
    {
        fakeCardObject.SetActive(true);

        Vector3 initialPos = fakeCardObject.transform.position;

        LeanTween.scale(fakeCardObject, new Vector3(2f, 2f, 2f), TRANSITION_TIME_CARD_EQUIP)
            .setFrom(new Vector3(1f, 1f, 1f))
            .setOnComplete(() =>
            {
                LeanTween.scale(fakeCardObject, new Vector3(1f, 1f, 1f), TRANSITION_TIME_CARD_EQUIP / 2f)
                .setFrom(new Vector3(2f, 2f, 2f))
                .setOnComplete(() =>
                {

                });

                LeanTween.move(fakeCardObject, target.position, TRANSITION_TIME_CARD_EQUIP / 2f)
                .setFrom(transform.position)
                .setOnComplete(() =>
                {
                    ResetEquip(initialPos);
                });
            });

        yield return new WaitForSeconds(TRANSITION_TIME_CARD_EQUIP * 1.5f);
    }

    private void SetUpForEquip(CardSO cardSO)
    {
        fakeCardObject.GetComponent<SpriteRenderer>().sprite = cardSO.image;
    }

    private void ResetEquip(Vector3 initialPos)
    {
        fakeCardObject.transform.position = initialPos;

        fakeCardObject.SetActive(false);
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return cardFrontImageOnField.GetComponent<SpriteRenderer>();
    }

    public IEnumerator SpecialSummon()
    {
        MonsterCard monsterCard = card as MonsterCard;

        CardNormalStateOnField();

        CardOnField();

        fakeCardObject.GetComponent<SpriteRenderer>().material = markAsTribute;

        fakeCardObject.SetActive(true);     

        LeanTween.color(fakeCardObject, new Color32(255, 255, 255, 0), TRANSITION_TIME_SPECIAL_SUMMON)
            .setFromColor(new Color32(255, 255, 255, 255))
            .setOnComplete(() =>
            {
                fakeCardObject.SetActive(false);

                fakeCardObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);

                fakeCardObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            });

        yield return new WaitForSeconds(TRANSITION_TIME_SPECIAL_SUMMON + DELAY_TIME_AFTER_SPECIAL_SUMMON);
    }

    public void SwitchSelectTargetOnField()
    {
        cardFrontImageOnField.sharedMaterial = cardFrontImageOnField.sharedMaterial == defaultMaterial ? activeMaterialOnField : defaultMaterial;
    }

    public IEnumerator SelectTargetOnSelect()
    {
        int currentTime = 0;

        float transitionTimerMax = 0.1f;

        float transitionTimer = 0f;

        SwitchSelectTargetOnField();

        while (currentTime < TIME_FOR_SELECT_TARGET)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer >= transitionTimerMax)
            {
                transitionTimer = 0f;

                currentTime++;

                SwitchSelectTargetOnField();
            }

            yield return null;
        }

        CardNormalStateOnField();
    }
}
