using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.VFX;

public struct CardData
{
    private int rank;  //0-8 represent ranks 2-10, 9 is Jack, 10 is Queen, 11 is King, and 12 is Ace
    private int suit; //0 is Club, 1 is Diamond, 2 is Heart, 3 is Spade

    public CardData(int r, int s)
    {
        rank = r;
        suit = s;
    }

    public int getRank()
    { 
        return rank; 
    }

    public int getSuit()
    {
        return suit;
    }

    public string getSuitName()
    {
        switch (suit)
        {
            case 0:
                return "Clubs";
                break;
            case 1:
                return "Diamonds";
                break;
            case 2:
                return "Hearts";
                break;
            case 3:
                return "Spades";
                break;
        }
        return "no suit";
    }

    public string getRankName() { 
        switch (rank) {
            case -1:
                return "no rank";
                break;
            case 9:
                return "Jack";
                break;
            case 10:
                return "Queen";
                break;
            case 11:
                return "King";
                break;
            case 12:
                return "Ace";
                break;
        }
        return rank + 2 + "";
    }
}

public class CardController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private GameObject cardPF;
    private GameController gameController;
    private PlayerController playerController;
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void createCard(Queue<CardData> deck) {
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector2 cardPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10);                                      // random position
        GameObject cardObject = Instantiate(cardPF, cardPos, new Quaternion(0, 0, 0, 0));                                 //create card
        CardData cardStruct = deck.Dequeue();                                                                             //Draws card from deck, sets to variable we will use twice
        cardObject.GetComponent<Card>().setCardData(cardStruct);                                                          // Sets data for our card to the card variable we made
        cardObject.GetComponent<SpriteRenderer>().sprite = cardSprites[cardStruct.getSuit() * 13 + cardStruct.getRank()]; //sets corresponding sprite based on card variable
        cardObject.GetComponent<Rigidbody2D>().velocity = new Vector2(cardPos.x / -10, cardPos.y / -10);                  //moves card toward player
    }
}
