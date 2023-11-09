/******************************************************************************
// File Name:       CardController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Contains a CardData struct to store information for a card.
                    Also contains code to instantiate each card in the scene.
******************************************************************************/
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

    // <summary>
    // The constructor for CardData
    // </summary>
    public CardData(int r, int s)
    {
        rank = r;
        suit = s;
    }

    // <summary>
    // Returns the rank of this card
    // </summary>
    // <returns> The rank of this card </returns>
    public int getRank()
    { 
        return rank; 
    }

    // <summary>
    // Returns the suit of this card
    // </summary>
    // <returns> The suit of this card </returns>
    public int getSuit()
    {
        return suit;
    }

    // <summary>
    // Returns the name of the suit of this card as a string
    // </summary>
    // <returns> the name of the suit of this card as a string </returns>
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

    // <summary>
    // Returns the name of the rank of this card as a string
    // </summary>
    // <returns> the name of the rank of this card as a string </returns>
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

    public bool isNull()
    {
        if (rank < 0 || suit < 0)
        {
            return true;
        }
        return false;
    }
}

public class CardController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private GameObject cardPF;
    private GameController gameController;
    private PlayerController playerController;

    // <summary>
    // Initializes our global variables
    // </summary>
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public Sprite[] getCardSprites() {
        return cardSprites;
    }

    // <summary>
    // Creates a card from a deck in the scene
    // </summary>
    public void createCard(Queue<CardData> deck) {
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector2 cardPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10);                                      // random position
        GameObject cardObject = Instantiate(cardPF, cardPos, new Quaternion(0, 0, 0, 0));                                 //create card
        CardData cardStruct = deck.Dequeue();                                                                             //Draws card from deck, sets to variable we will use twice
        cardObject.GetComponent<Card>().setCardData(cardStruct);                                                          // Sets data for our card to the card variable we made
        cardObject.GetComponent<SpriteRenderer>().sprite = cardSprites[cardStruct.getSuit() * 13 + cardStruct.getRank()]; //sets corresponding sprite based on card variable
    }
}
