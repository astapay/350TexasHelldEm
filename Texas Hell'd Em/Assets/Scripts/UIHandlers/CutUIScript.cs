using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
* ******************************************************************
File name: CutUiScript
author: David Henvick
Creation date: 11/8/23
summary: this is the script that is used swap out the ui for the cutscene when cards are updated
*/
public class CutUiScript : MonoBehaviour
{
    GameController gameController;
    CardController cardController;
    PlayerController playerController;

    //player cards
    [SerializeField] private GameObject playerCard1;
    [SerializeField] private GameObject playerCard2;
    //river cards
    [SerializeField] private GameObject riverCard1;
    [SerializeField] private GameObject riverCard2;
    [SerializeField] private GameObject riverCard3;
    [SerializeField] private GameObject riverCard4;
    [SerializeField] private GameObject riverCard5;

    //npc1 cards
    [SerializeField] private GameObject bot1Card1;
    [SerializeField] private GameObject bot1Card2;
    //npc2 cards
    [SerializeField] private GameObject bot2Card1;
    [SerializeField] private GameObject bot2Card2;
    //npc3 cards
    [SerializeField] private GameObject bot3Card1;
    [SerializeField] private GameObject bot3Card2;

    //player indicator
    [SerializeField] private GameObject PlayerIndicator;
    //npc indicators
    [SerializeField] private GameObject NPC1Indicator;
    [SerializeField] private GameObject NPC2Indicator;
    [SerializeField] private GameObject NPC3Indicator;

    //player card sprites
    private Sprite playerCard1Sprite;
    private Sprite playerCard2Sprite;
    //river card sprites
    private Sprite riverCard1Sprite;
    private Sprite riverCard2Sprite;
    private Sprite riverCard3Sprite;
    private Sprite riverCard4Sprite;
    private Sprite riverCard5Sprite;

    //npc1 card sprites
    private Sprite bot1Card1Sprite;
    private Sprite bot1Card2Sprite;
    //npc2 card sprites
    private Sprite bot2Card1Sprite;
    private Sprite bot2Card2Sprite;
    //npc3 card sprites
    private Sprite bot3Card1Sprite;
    private Sprite bot3Card2Sprite;

    //player indicator sprites
    private Sprite PlayerIndicatorCard;
    private Sprite PlayerIndicatorSuit;
    private Sprite PlayerIndicatorHand;

    //npc indicator sprites
    private Sprite NPC1IndicatorCard;
    private Sprite NPC1IndicatorSuit;
    private Sprite NPC1IndicatorHand;

    private Sprite NPC2IndicatorCard;
    private Sprite NPC2IndicatorSuit;
    private Sprite NPC2IndicatorHand;

    private Sprite NPC3IndicatorCard;
    private Sprite NPC3IndicatorSuit;
    private Sprite NPC3IndicatorHand;

    //counter
    int counter;

    //ui
    [SerializeField] private GameObject retryBtn;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject quitBtn;

    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text WinLoseText;

    public bool win;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
        cardController = FindObjectOfType<CardController>();

        //get cards from game
        SetSprites();
        //check win
        CheckWin();
        //set player cards
        playerCard1.GetComponent<SpriteRenderer>().sprite = playerCard1Sprite;
        playerCard2.GetComponent<SpriteRenderer>().sprite = playerCard2Sprite;
        //set river cards
        riverCard1.GetComponent<SpriteRenderer>().sprite = riverCard1Sprite;
        riverCard2.GetComponent<SpriteRenderer>().sprite = riverCard2Sprite;
        riverCard3.GetComponent<SpriteRenderer>().sprite = riverCard3Sprite;
        riverCard4.GetComponent<SpriteRenderer>().sprite = riverCard4Sprite;
        riverCard5.GetComponent<SpriteRenderer>().sprite = riverCard5Sprite;

        NPC1Indicator.SetActive(false);
        NPC2Indicator.SetActive(false);
        NPC3Indicator.SetActive(false);

        retryBtn.SetActive(false);
        menuBtn.SetActive(false);
        quitBtn.SetActive(false);
        WinLoseText.gameObject.SetActive(false);

        counter = 0;

        score.SetText(gameController.chipCounter.ToString());
    }

    private void SetSprites()
    {
        playerCard1Sprite = cardController.getCardSprite(playerController.getHand()[0]);
        playerCard2Sprite = cardController.getCardSprite(playerController.getHand()[1]);
        riverCard1Sprite = cardController.getCardSprite(gameController.getRiver()[0]);
        riverCard2Sprite = cardController.getCardSprite(gameController.getRiver()[1]);
        riverCard3Sprite = cardController.getCardSprite(gameController.getRiver()[2]);
        riverCard4Sprite = cardController.getCardSprite(gameController.getRiver()[3]);
        riverCard5Sprite = cardController.getCardSprite(gameController.getRiver()[4]);
        bot1Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[0][0]);
        bot1Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[0][1]);
        bot2Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[1][0]);
        bot2Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[1][1]);
        bot3Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[2][0]);
        bot3Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[2][1]);
    }

    private void CheckWin()
    {
        if (win)
        {
            WinLoseText.SetText("You Win!!");
        }
        else
        {
            WinLoseText.SetText("You Lost!!");
        }
    }

    private void FixedUpdate()
    {
        counter++;

        if(counter > 40)
        {
            NPC1Indicator.SetActive(true);
            bot1Card1.GetComponent<SpriteRenderer>().sprite = bot1Card1Sprite;
            bot2Card1.GetComponent<SpriteRenderer>().sprite = bot2Card2Sprite;
        }
        else if (counter > 80)
        {
            NPC2Indicator.SetActive(true);
            bot2Card1.GetComponent<SpriteRenderer>().sprite = bot2Card1Sprite;
            bot2Card2.GetComponent<SpriteRenderer>().sprite = bot2Card2Sprite;
        }
        else if (counter > 120)
        {
            NPC3Indicator.SetActive(true);
            bot3Card1.GetComponent<SpriteRenderer>().sprite = bot3Card1Sprite;
            bot3Card2.GetComponent<SpriteRenderer>().sprite = bot3Card2Sprite;
        }
        else if (counter > 150)
        {
            WinLoseText.gameObject.SetActive(true);
            retryBtn.SetActive(true);
            menuBtn.SetActive(true);
            quitBtn.SetActive(true);
        }
    }
}
