using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] GameController gameController;
    [SerializeField] CardController cardController;
    [SerializeField] PlayerController playerController;

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
    [SerializeField] private GameObject NPC1Card1;
    [SerializeField] private GameObject NPC1Card2;
    //npc2 cards
    [SerializeField] private GameObject NPC2Card1;
    [SerializeField] private GameObject NPC2Card2;
    //npc3 cards
    [SerializeField] private GameObject NPC3Card1;
    [SerializeField] private GameObject NPC3Card2;

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
    private Sprite NPC1Card1Sprite;
    private Sprite NPC1Card2Sprite;
    //npc2 card sprites
    private Sprite NPC2Card1Sprite;
    private Sprite NPC2Card2Sprite;
    //npc3 card sprites
    private Sprite NPC3Card1Sprite;
    private Sprite NPC3Card2Sprite;

    //counter
    int counter;
    bool isActive = false;

    //ui
    [SerializeField] private GameObject retryBtn;
    [SerializeField] private GameObject menuBtn;

    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text WinLoseText;

    private int winner;
    private int winnerHand;
    private int endScore;

    [SerializeField] private Sprite straightFlush;
    [SerializeField] private Sprite fourOfAKind;
    [SerializeField] private Sprite fullHouse;
    [SerializeField] private Sprite flush;
    [SerializeField] private Sprite straight;
    [SerializeField] private Sprite threeOfAKind;
    [SerializeField] private Sprite twoPair;
    [SerializeField] private Sprite pair;
    [SerializeField] private Sprite highCard;


    /// <summary>
    /// called on start
    /// </summary>
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
        cardController = FindObjectOfType<CardController>();
    }

    /// <summary>
    /// called on game end and cutscene begin
    /// </summary>
    public void Activate()
    {
        isActive = true;
        //get cards from game
        SetSprites();
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
        WinLoseText.gameObject.SetActive(false);

        //check win
        CheckWin();
        //sets indicator of winner
        if (winner != 0)
        {
            SetAIIndicator();
        }

        counter = 0;
    }

    /// <summary>
    /// sets the sprites to the sprites of the cards that the player,river, and ai have
    /// </summary>
    private void SetSprites()
    {
        playerCard1Sprite = cardController.getCardSprite(playerController.getHand()[0]);
        playerCard2Sprite = cardController.getCardSprite(playerController.getHand()[1]);
        riverCard1Sprite = cardController.getCardSprite(gameController.getRiver()[0]);
        riverCard2Sprite = cardController.getCardSprite(gameController.getRiver()[1]);
        riverCard3Sprite = cardController.getCardSprite(gameController.getRiver()[2]);
        riverCard4Sprite = cardController.getCardSprite(gameController.getRiver()[3]);
        riverCard5Sprite = cardController.getCardSprite(gameController.getRiver()[4]);
        NPC1Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[0][0]);
        NPC1Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[0][1]);
        NPC2Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[1][0]);
        NPC2Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[1][1]);
        NPC3Card1Sprite = cardController.getCardSprite(gameController.getAIHands()[2][0]);
        NPC3Card2Sprite = cardController.getCardSprite(gameController.getAIHands()[2][1]);
    }

    private void SetAIIndicator()
    {
        if(winner == 1)
        {
            NPC1Indicator.GetComponent<SpriteRenderer>().sprite = GetIndicatorSprite(winnerHand);
        }
        else if (winner == 2)
        {
            NPC2Indicator.GetComponent<SpriteRenderer>().sprite = GetIndicatorSprite(winnerHand);
        }
        else
        {
            NPC3Indicator.GetComponent<SpriteRenderer>().sprite = GetIndicatorSprite(winnerHand);
        }
    }
    /// <summary>
    /// sets win text and checks if win
    /// </summary>
    private void CheckWin()
    {
        endScore = gameController.chipCounter;

        winner = gameController.winnerDetails.Item1;
        winnerHand = gameController.winnerDetails.Item2;

        if(winner == 0)
        {
            WinLoseText.SetText("You have won!");
            PlayerIndicator.GetComponent<SpriteRenderer>().sprite = GetIndicatorSprite(winnerHand);
            endScore = endScore * winnerHand;
        }
        else
        {
            WinLoseText.SetText("You have lost!");
            endScore = 0;
        }
        score.SetText(endScore.ToString());
    }

    /// <summary>
    /// takes an int for the hand and returns the sprite that matches the int
    /// </summary>
    /// <param name="Hand"></param> int
    /// <returns></returns> sprite
    private Sprite GetIndicatorSprite(int Hand)
    {
        if (Hand == 0)
        {
            return highCard;
        }
        else if (Hand == 1)
        {
            return pair;
        }
        else if (Hand == 2)
        {
            return twoPair;
        }
        else if (Hand == 3)
        {
            return threeOfAKind;
        }
        else if (Hand == 4)
        {
            return straight;
        }
        else if (Hand == 5)
        {
            return flush;
        }
        else if (Hand == 6)
        {
            return fullHouse;
        }
        else if (Hand == 7)
        {
            return fourOfAKind;
        }
        else if (Hand == 8)
        {
            return straightFlush;
        }
        else 
        {
            return highCard;
        }
    }

    /// <summary>
    /// called 50 times a second
    /// </summary>
    private void FixedUpdate()
    {
        if (isActive)
        {
            counter++;
            Debug.Log(counter);

            if (counter == 50)
            {
                if(winner == 1)
                {
                    NPC1Indicator.SetActive(true);
                }
                NPC1Card1.GetComponent<SpriteRenderer>().sprite = NPC1Card1Sprite;
                NPC1Card2.GetComponent<SpriteRenderer>().sprite = NPC1Card2Sprite;
            }
            else if (counter == 100)
            {
                if (winner == 2)
                {
                    NPC2Indicator.SetActive(true);
                }
                NPC2Card1.GetComponent<SpriteRenderer>().sprite = NPC2Card1Sprite;
                NPC2Card2.GetComponent<SpriteRenderer>().sprite = NPC2Card2Sprite;
            }
            else if (counter == 150)
            {
                if (winner == 3)
                {
                    NPC3Indicator.SetActive(true);
                }
                NPC3Card1.GetComponent<SpriteRenderer>().sprite = NPC3Card1Sprite;
                NPC3Card2.GetComponent<SpriteRenderer>().sprite = NPC3Card2Sprite;
            }
            else if (counter == 250)
            {
                WinLoseText.gameObject.SetActive(true);
                retryBtn.SetActive(true);
                menuBtn.SetActive(true);
            }
        }
    }
}
