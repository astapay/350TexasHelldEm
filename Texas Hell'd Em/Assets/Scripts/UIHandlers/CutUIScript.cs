using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        //get cards from game
        GetCardSprites();
        //set player cards
        playerCard1.GetComponent<SpriteRenderer>().sprite = playerCard1Sprite;
        playerCard2.GetComponent<SpriteRenderer>().sprite = playerCard2Sprite;
        //set river cards
        riverCard1.GetComponent<SpriteRenderer>().sprite = riverCard1Sprite;
        riverCard2.GetComponent<SpriteRenderer>().sprite = riverCard2Sprite;
        riverCard3.GetComponent<SpriteRenderer>().sprite = riverCard3Sprite;
        riverCard4.GetComponent<SpriteRenderer>().sprite = riverCard4Sprite;
        riverCard5.GetComponent<SpriteRenderer>().sprite = riverCard5Sprite;

    }

    private void GetCardSprites()
    {
        Sprite[] spriteSheet = gameController.GetCardSprites();
        playerCard1Sprite = spriteSheet[1];
        playerCard2Sprite = spriteSheet[2];
        riverCard1Sprite = spriteSheet[3];
        riverCard2Sprite = spriteSheet[4];
        riverCard3Sprite = spriteSheet[5];
        riverCard4Sprite = spriteSheet[6];
        riverCard5Sprite = spriteSheet[7];
        bot1Card1Sprite = spriteSheet[8];
        bot1Card2Sprite = spriteSheet[9];
        bot2Card1Sprite = spriteSheet[10];
        bot2Card2Sprite = spriteSheet[11]; 
        bot3Card1Sprite = spriteSheet[12];
        bot3Card2Sprite = spriteSheet[13];
    }
}
